using Firebase.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_v1.Data;
using Project_v1.Models;
using Project_v1.Models.DTOs.Helper;
using Project_v1.Models.DTOs.Response;
using Project_v1.Models.DTOs.SampleCount;
using Project_v1.Models.DTOs.Summary;
using Project_v1.Models.DTOs.WCReport;
using Project_v1.Services.Filtering;
using Project_v1.Services.IdGeneratorService;
using Project_v1.Services.ReportService;
using System.Diagnostics.Metrics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Project_v1.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class WCSampleController : ControllerBase {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<SystemUser> _userManager;
        private readonly IReportService _reportService;
        private readonly IIdGenerator _idGenerator;
        private readonly IFilter _filter;
        private readonly HttpClient _httpClient;
        private readonly string _googleMapsApiKey;
        private readonly IConfiguration _configuration;

        public WCSampleController(ApplicationDBContext context,
                                  UserManager<SystemUser> userManager,
                                  IReportService reportService,
                                  IIdGenerator idGenerator,
                                  IFilter filter,
                                  HttpClient httpClient,
                                  IConfiguration configuration) {
            _context = context;
            _userManager = userManager;
            _reportService = reportService;
            _idGenerator = idGenerator;
            _filter = filter;
            _httpClient = httpClient;
            _googleMapsApiKey = configuration["ApiKeys:GoogleMapsApi"];
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetCities")]
        public async Task<IActionResult> GetCities(String query) {
            try {
                var country = "LK";
                var requestUrl = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={query}&components=country:{country}&types=(cities)&key={_googleMapsApiKey}";

                var response = await _httpClient.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode) {
                    var data = await response.Content.ReadAsStringAsync();
                    return Ok(data);
                }
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            } catch (HttpRequestException e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetWCSamples")]
        public async Task<IActionResult> GetWCSamples() {
            try {
                var samples = await _context.Samples.ToListAsync();
                return Ok(samples);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("getAddedSamples")]
        public async Task<IActionResult> GetAddedSamples([FromQuery] QueryObject query) {
            try {
                if (query.UserId == null) {
                    return NotFound();
                }

                var phi = await _userManager.FindByIdAsync(query.UserId);

                if (phi == null) {
                    return NotFound($"User with username '{query.UserId}' not found.");
                }

                if (phi.PHIAreaId == null) {
                    return NotFound($"User with username '{query.UserId}' have a PHI Area assigned.");
                }

                var samples = _context.Samples
                    .Where(sample => sample.PhiId == query.UserId && sample.Acceptance == "Pending")
                    .Select(sample => new {
                        sample.SampleId,
                        sample.YourRefNo,
                        sample.StateOfChlorination,
                        sample.DateOfCollection,
                        sample.CatagoryOfSource,
                        sample.CollectingSource,
                        sample.phiAreaName,
                        sample.Acceptance,
                        sample.Comments
                    });

                /*var searchResults = _filter.Search(samples, query.YourRefNo, "YourRefNo");
                var sortedResult = _filter.Sort(searchResults, query);
                var result = await _filter.Paginate(sortedResult, query.PageNumber, query.PageSize);*/

                var filteredResult = await _filter.Filtering(samples, query);

                return Ok(filteredResult);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetHistory")]
        public async Task<IActionResult> GetHistory([FromQuery] QueryObject query) {
            try {
                if (query.UserId == null) {
                    return NotFound();
                }

                var phi = await _userManager.FindByIdAsync(query.UserId);

                if (phi == null) {
                    return NotFound($"User with username '{query.UserId}' not found.");
                }

                if (phi.PHIAreaId == null) {
                    return NotFound($"User with username '{query.UserId}' have a PHI Area assigned.");
                }

                var samples = _context.Samples
                    .Where(sample => sample.PhiId == query.UserId && (sample.Acceptance == "Accepted" || sample.Acceptance == "Rejected"))
                    .Select(sample => new {
                        sample.SampleId,
                        sample.YourRefNo,
                        sample.StateOfChlorination,
                        sample.DateOfCollection,
                        sample.CatagoryOfSource,
                        sample.CollectingSource,
                        sample.phiAreaName,
                        sample.Acceptance,
                        sample.Comments
                    });

                /*var searchResults = _filter.Search(samples, query.YourRefNo, "YourRefNo");
                var sortedResult = _filter.Sort(searchResults, query);
                var result = await _filter.Paginate(sortedResult, query.PageNumber, query.PageSize);*/

                var filteredResult = await _filter.Filtering(samples, query);

                return Ok(filteredResult);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetPendingSamples")]
        public async Task<IActionResult> GetPendingSamples([FromQuery] QueryObject query) {
            try {
                if (query.UserId == null) {
                    return NotFound();
                }

                var mlt = await _userManager.FindByIdAsync(query.UserId);

                if (mlt == null) {
                    return NotFound($"User with username '{query.UserId}' not found.");
                }

                if (mlt.LabID == null) {
                    return NotFound($"User with username '{query.UserId}' have a Lab assigned.");
                }

                /*var mohArea = await _context.MOHAreas.Where(m => m.LabID == mlt.LabID).ToListAsync();

                if (mohArea == null) {
                    return NotFound($"No MOHArea found for the Lab assigned to user '{query.UserId}'.");
                }

                var mohAreaIds = mohArea.Select(m => m.MOHAreaID).ToList();

                var phiAreas = await _context.PHIAreas.Where(phi => mohAreaIds
                    .Contains(phi.MOHAreaId))
                    .ToListAsync();

                var phiAreaIds = phiAreas.Select(pa => pa.PHIAreaID).ToList();*/

                /*var samples = _context.Samples
                    .Where(sample => phiAreaIds
                    .Contains(sample.PHIAreaId) && sample.Acceptance == "Pending")
                    .Select(sample => new {
                        sample.SampleId,
                        sample.YourRefNo,
                        sample.StateOfChlorination,
                        sample.DateOfCollection,
                        sample.AnalyzedDate,
                        sample.CatagoryOfSource,
                        sample.CollectingSource,
                        sample.phiAreaName,
                        sample.Acceptance,
                        sample.PHIArea.MOHArea.LabID,
                        sample.PHIArea.MOHArea.Lab.LabName,
                        sample.PHIArea.MOHArea.Lab.LabLocation,
                        sample.PHIArea.MOHArea.Lab.LabTelephone,
                        ReportAvailable = _context.Reports.Any(r => r.SampleId == sample.SampleId)
                    });*/

                var samples = _context.Samples
                    .Where(sample => sample.PHIArea.MOHArea.LabID == mlt.LabID && sample.Acceptance == "Pending")
                    .Select(sample => new {
                        sample.SampleId,
                        sample.YourRefNo,
                        sample.StateOfChlorination,
                        sample.DateOfCollection,
                        sample.AnalyzedDate,
                        sample.CatagoryOfSource,
                        sample.CollectingSource,
                        sample.phiAreaName,
                        sample.Acceptance,
                        sample.PHIArea.MOHArea.LabID,
                        sample.PHIArea.MOHArea.Lab.LabName,
                        sample.PHIArea.MOHArea.Lab.LabLocation,
                        sample.PHIArea.MOHArea.Lab.LabTelephone,
                        ReportAvailable = _context.Reports.Any(r => r.SampleId == sample.SampleId)
                    });

                /*var searchResults = _filter.Search(samples, query.YourRefNo, "YourRefNo");
                var sortedResult = _filter.Sort(searchResults, query);
                var result = await _filter.Paginate(sortedResult, query.PageNumber, query.PageSize);*/

                var filteredResult = await _filter.Filtering(samples, query);

                return Ok(filteredResult);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetAcceptedSamples")]
        public async Task<IActionResult> GetAcceptedSamples([FromQuery] QueryObject query) {
            try {
                if (query.UserId == null) {
                    return NotFound();
                }

                var mlt = await _userManager.FindByIdAsync(query.UserId);

                if (mlt == null) {
                    return NotFound($"User with username '{query.UserId}' not found.");
                }

                if (mlt.LabID == null) {
                    return NotFound($"User with username '{query.UserId}' have a Lab assigned.");
                }

                /*var mohArea = await _context.MOHAreas.Where(m => m.LabID == mlt.LabID).ToListAsync();

                if (mohArea == null) {
                    return NotFound($"No MOHArea found for the Lab assigned to user '{query.UserId}'.");
                }

                var mohAreaIds = mohArea.Select(m => m.MOHAreaID).ToList();*/

                /*var phiAreas = await _context.PHIAreas.Where(phi => mohAreaIds
                    .Contains(phi.MOHAreaId))
                    .ToListAsync();

                var phiAreaIds = phiAreas.Select(pa => pa.PHIAreaID).ToList();*/

                /*var mohAreaIds = await _context.MOHAreas
                    .Where(m => m.LabID == mlt.LabID)
                    .Select(m => m.MOHAreaID)
                    .ToListAsync();

                if (!mohAreaIds.Any()) {
                    return NotFound($"No MOHArea found for the Lab assigned to user '{query.UserId}'.");
                }

                var phiAreaIds = await _context.PHIAreas.Where(phi => mohAreaIds
                    .Contains(phi.MOHAreaId))
                    .Select(pa => pa.PHIAreaID)
                    .ToListAsync();

                if (!phiAreaIds.Any()) {
                    return NotFound($"No PHIArea found for the MOHAreas assigned to the Lab of user '{query.UserId}'.");
                }*/

                /*var samples = _context.Samples
                    .Where(sample => phiAreaIds
                    .Contains(sample.PHIAreaId) && sample.Acceptance == "Accepted")
                    .Select(sample => new {
                        sample.SampleId,
                        sample.YourRefNo,
                        sample.StateOfChlorination,
                        sample.DateOfCollection,
                        sample.AnalyzedDate,
                        sample.CatagoryOfSource,
                        sample.CollectingSource,
                        sample.phiAreaName,
                        sample.Acceptance,
                        sample.PHIArea.MOHArea.LabID,
                        sample.PHIArea.MOHArea.Lab.LabName,
                        sample.PHIArea.MOHArea.Lab.LabLocation,
                        sample.PHIArea.MOHArea.Lab.LabTelephone,
                        ReportAvailable = _context.Reports.Any(r => r.SampleId == sample.SampleId)
                    });*/

                var samples = _context.Samples
                    .Where(s => s.PHIArea.MOHArea.LabID == mlt.LabID && s.Acceptance == "Accepted")
                    .Select(sample => new {
                        sample.SampleId,
                        sample.YourRefNo,
                        sample.StateOfChlorination,
                        sample.DateOfCollection,
                        sample.AnalyzedDate,
                        sample.CatagoryOfSource,
                        sample.CollectingSource,
                        sample.phiAreaName,
                        sample.Acceptance,
                        sample.PHIArea.MOHArea.LabID,
                        sample.PHIArea.MOHArea.Lab.LabName,
                        sample.PHIArea.MOHArea.Lab.LabLocation,
                        sample.PHIArea.MOHArea.Lab.LabTelephone,
                        ReportAvailable = _context.Reports.Any(r => r.SampleId == sample.SampleId)
                    });

                /*var searchResults = _filter.Search(samples, query.YourRefNo, "YourRefNo");
                var sortedResult = _filter.Sort(searchResults, query);
                var result = await _filter.Paginate(sortedResult, query.PageNumber, query.PageSize);*/

                var filteredResult = await _filter.Filtering(samples, query);

                return Ok(filteredResult);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetSampleCount")]
        public async Task<IActionResult> GetSampleCount([FromQuery] QueryObject query) {
            try {
                if (query.UserId == null) {
                    return NotFound();
                }

                var mlt = await _userManager.FindByIdAsync(query.UserId);

                if (mlt == null) {
                    return NotFound($"User with username '{query.UserId}' not found.");
                }

                if (mlt.LabID == null) {
                    return NotFound($"User with username '{query.UserId}' does not have a Lab assigned.");
                }

                var samples = _context.Samples
                    .Include(s => s.PHIArea)
                    .ThenInclude(p => p.MOHArea)
                    .Where(s => s.PHIArea.MOHArea.LabID == mlt.LabID)
                    .GroupBy(s => new {
                        s.DateOfCollection.Year,
                        s.DateOfCollection.Month,
                        s.PHIArea.MOHArea.MOHAreaName
                    })
                    .Select(g => new {
                        g.Key.Year,
                        g.Key.Month,
                        g.Key.MOHAreaName,
                        SampleCount = g.Count()
                    });

                var filteredSamples = await _filter.Filtering(samples, query);

                var groupedSamples = filteredSamples.Items
                     .GroupBy(s => s.Year)
                     .Select(g => new SampleCountResponse {
                         Year = g.Key,
                         Months = g.GroupBy(m => m.Month)
                                   .Select(m => new MonthSampleCount {
                                       Month = m.Key,
                                       MOHSampleCounts = m.Select(moh => new MOHSampleCount {
                                           MOHAreaName = moh.MOHAreaName,
                                           SampleCount = moh.SampleCount
                                       }).ToList()
                                   }).ToList()
                     });

                var totalPages = filteredSamples.TotalPages;

                return Ok(new {
                    groupedSamples,
                    filteredSamples.TotalPages
                });

            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetMonthlySamples")]
        public async Task<IActionResult> GetMonthlySamples([FromQuery] QueryObject query, [FromQuery] int? Month = null, [FromQuery] int? Year = null) {
            try {
                if (query.UserId == null) {
                    return NotFound();
                }

                var mlt = await _userManager.FindByIdAsync(query.UserId);

                if (mlt == null) {
                    return NotFound($"User with username '{query.UserId}' not found.");
                }

                if (mlt.LabID == null) {
                    return NotFound($"User with username '{query.UserId}' does not have a Lab assigned.");
                }

                /*var samples = _context.PHIAreas
                    .Where(p => p.MOHArea.LabID == mlt.LabID) // Filter PHI areas by LabID
                    .SelectMany(p => p.Samples.DefaultIfEmpty(), (p, s) => new { PHIArea = p, Sample = s }) // Perform left outer join with samples
                    .GroupBy(ps => new {
                        Year = ps.Sample != null ? ps.Sample.DateOfCollection.Year : (int?)null,
                        Month = ps.Sample != null ? ps.Sample.DateOfCollection.Month : (int?)null,
                        ps.PHIArea.PHIAreaName,
                        ps.PHIArea.MOHArea.MOHAreaName
                    })
                    .Select(g => new {
                        g.Key.Year,
                        g.Key.Month,
                        g.Key.PHIAreaName,
                        g.Key.MOHAreaName,
                        SamplesReceived = g.Key.Year != null, // Check if any samples are received for this PHI Area
                        SampleCount = g.Count(s => s.Sample != null) // Calculate the sample count
                    });

                var filteredSamples = await _filter.Filtering(samples, query);

                var groupedSamples = filteredSamples.Items
                    .GroupBy(s => s.Year)
                    .Select(g => new {
                        Year = g.Key,
                        Months = g.GroupBy(m => m.Month)
                                   .Select(m => new {
                                       Month = m.Key,
                                       MOHSampleCounts = m.GroupBy(moh => new { moh.MOHAreaName, moh.PHIAreaName })
                                                          .Select(mohGroup => new {
                                                              mohGroup.Key.MOHAreaName,
                                                              mohGroup.Key.PHIAreaName,
                                                              SamplesReceived = mohGroup.Any(moh => moh.SamplesReceived),
                                                              SampleCount = mohGroup.Sum(moh => moh.SampleCount)
                                                          }).ToList()
                                   }).ToList()
                    });*/

                var phiAreasList = _context.PHIAreas
                    .Select(phiaArea => new {
                        phiAreaId = phiaArea.PHIAreaID,
                        phiAreaName = phiaArea.PHIAreaName,
                        mohAreaName = phiaArea.MOHArea.MOHAreaName,
                        sampleCount = _context.Samples
                            .Where(sample => sample.PHIAreaId == phiaArea.PHIAreaID &&
                                             (Month == null || sample.DateOfCollection.Month == Month) &&
                                             (Year == null || sample.DateOfCollection.Year == Year))
                            .Count(),
                        acceptedSampleCount = _context.Samples
                            .Where(sample => sample.PHIAreaId == phiaArea.PHIAreaID &&
                                             (Month == null || sample.DateOfCollection.Month == Month) &&
                                             (Year == null || sample.DateOfCollection.Year == Year) &&
                                             sample.Acceptance == "Accepted")
                            .Count(),
                        rejectedSampleCount = _context.Samples
                            .Where(sample => sample.PHIAreaId == phiaArea.PHIAreaID &&
                                             (Month == null || sample.DateOfCollection.Month == Month) &&
                                             (Year == null || sample.DateOfCollection.Year == Year) &&
                                             sample.Acceptance == "Rejected")
                            .Count(),
                        pendingSampleCount = _context.Samples
                            .Where(sample => sample.PHIAreaId == phiaArea.PHIAreaID &&
                                             (Month == null || sample.DateOfCollection.Month == Month) &&
                                             (Year == null || sample.DateOfCollection.Year == Year) &&
                                             sample.Acceptance == "Pending")
                            .Count()
                    });

                var filteredSamples = await _filter.Filtering(phiAreasList, query);

                return Ok(filteredSamples);

            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("GetSampleCountReport")]
        public async Task<IActionResult> GetSampleCountReport([FromQuery] String MltId, [FromQuery] int Year) {
            try {
                if (MltId == null) {
                    return NotFound();
                }

                var mlt = await _userManager.FindByIdAsync(MltId);

                if (mlt == null) {
                    return NotFound($"User with username '{MltId}' not found.");
                }

                if (mlt.LabID == null) {
                    return NotFound($"User with username '{MltId}' does not have a Lab assigned.");
                }

                var samples = await _context.Samples
                .Include(s => s.PHIArea)
                .ThenInclude(p => p.MOHArea)
                .Where(s => s.PHIArea.MOHArea.LabID == mlt.LabID && s.DateOfCollection.Year == Year)
                .GroupBy(s => new { s.DateOfCollection.Year, s.DateOfCollection.Month, s.PHIArea.MOHArea.MOHAreaName })
                .Select(g => new SampleCount {
                    MOHAreaName = g.Key.MOHAreaName,
                    TotalCount = g.Count(),
                    Month = g.Key.Month,
                    Year = g.Key.Year
                }).ToListAsync();

                byte[] report = _reportService.GenerateSampleCountReport(samples, Year);

                return File(report, "application/pdf", "fullSampleCount - " + Year);
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPost]
        [Route("AddWCSample")]
        public async Task<IActionResult> AddWCSample([FromBody] Wc_sample wcsample) {
            try {
                if (wcsample == null || !ModelState.IsValid) {
                    return BadRequest(new Response { Status = "Error", Message = "Invalid data!" });
                }

                var sample = new Sample {
                    SampleId = _idGenerator.GenerateSampleId(),
                    YourRefNo = wcsample.YourRefNo,
                    StateOfChlorination = wcsample.StateOfChlorination,
                    DateOfCollection = wcsample.DateOfCollection,
                    CatagoryOfSource = wcsample.CatagoryOfSource,
                    CollectingSource = wcsample.CollectingSource,
                    Acceptance = "Pending",
                    PhiId = wcsample.phiId,
                    phiAreaName = wcsample.phiAreaName,
                    PHIAreaId = wcsample.PHIAreaID,
                    Comments = ""
                };

                await _context.Samples.AddAsync(sample);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "WC Sample added successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("updateWCSample/{id}")]
        public async Task<IActionResult> UpdateWCSample([FromRoute] String id, [FromBody] Wc_updatedSample updatedSample) {
            try {
                var sample = await _context.Samples.FindAsync(id);

                if (sample == null) {
                    return NotFound(new Response { Status = "Error", Message = "Sample not found!" });
                }

                if (updatedSample == null || !ModelState.IsValid) {
                    return BadRequest(new Response { Status = "Error", Message = "Invalid sample data!" });
                }

                sample.YourRefNo = updatedSample.YourRefNo;
                sample.StateOfChlorination = updatedSample.StateOfChlorination;
                sample.DateOfCollection = updatedSample.DateOfCollection;
                sample.CatagoryOfSource = updatedSample.CatagoryOfSource;
                sample.CollectingSource = updatedSample.CollectingSource;

                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Sample updated successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpPut]
        [Route("updateSampleStatus")]
        public async Task<IActionResult> UpdateSampleStatus([FromBody] SampleStatus sampleStatus) {
            try {
                var sampleToUpdate = await _context.Samples.FindAsync(sampleStatus.SampleId);

                if (sampleToUpdate == null) {
                    return NotFound(new Response { Status = "Error", Message = "Sample not found!" });
                }

                sampleToUpdate.Acceptance = sampleStatus.Status;
                sampleToUpdate.Comments = sampleStatus.Comment;

                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Sample status updated successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        [Route("deleteWCSample/{id}")]
        public async Task<IActionResult> DeleteWCSample([FromRoute] String id) {
            try {
                var sample = await _context.Samples.FindAsync(id);

                if (sample == null) {
                    return NotFound(new Response { Status = "Error", Message = "Sample not found!" });
                }

                _context.Samples.Remove(sample);
                await _context.SaveChangesAsync();

                return Ok(new Response { Status = "Success", Message = "Sample deleted successfully!" });
            } catch (Exception e) {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
