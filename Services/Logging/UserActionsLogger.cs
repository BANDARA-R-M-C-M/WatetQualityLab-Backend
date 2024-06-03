using Serilog.Core;

namespace Project_v1.Services.Logging {
    public class UserActionsLogger {
        private readonly Logger _userActionLogger;

        public UserActionsLogger(Logger userActionLogger) {
            _userActionLogger = userActionLogger;
        }

        public void LogInformation(string message) {
            _userActionLogger.Information(message);
        }
    }
}
