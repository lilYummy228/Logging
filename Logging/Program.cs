using System;
using System.IO;

namespace Lesson
{
    class Program
    {
        static void Main(string[] args)
        {
            Pathfinder fileLogWritter = new Pathfinder
                (new FileLogWritter());

            Pathfinder consoleLogWritter = new Pathfinder
                (new ConsoleLogWritter());

            Pathfinder fridayFileLogWritter = new Pathfinder
                (new DateLogWritter
                (new FileLogWritter(), DayOfWeek.Friday));

            Pathfinder fridayConsoleLogWritter = new Pathfinder
                (new DateLogWritter
                (new ConsoleLogWritter(), DayOfWeek.Friday));

            Pathfinder totalLogWritter = new Pathfinder
                (new TotalLogWritter
                (new ConsoleLogWritter(), new DateLogWritter
                (new FileLogWritter(), DayOfWeek.Friday)));
                
        }
    }

    internal interface ILogger
    {
        void WriteError(string message);
    }

    class Pathfinder
    {
        private const string Message = "error";
        private readonly ILogger _logger;

        public Pathfinder(ILogger logger) =>
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public void Find() =>
            _logger.WriteError(Message);
    }

    class ConsoleLogWritter : ILogger
    {
        public void WriteError(string message) =>
            Console.WriteLine(message);
    }

    class FileLogWritter : ILogger
    {
        private const string LogText = "log.txt";

        public void WriteError(string message) =>
            File.WriteAllText(LogText, message);
    }

    class DateLogWritter : ILogger
    {
        private readonly ILogger _logger;
        private readonly DayOfWeek _dayOfWeek;

        public DateLogWritter(ILogger logger, DayOfWeek dayOfWeek)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dayOfWeek = dayOfWeek;
        }

        public void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == _dayOfWeek)
                _logger.WriteError(message);
        }
    }

    class TotalLogWritter : ILogger
    {
        private readonly ILogger[] _loggers;

        public TotalLogWritter(params ILogger[] loggers) => 
            _loggers = loggers ?? throw new ArgumentNullException(nameof(loggers));

        public void WriteError(string message)
        {
            foreach (ILogger logger in _loggers)
                logger.WriteError(message);
        }
    }
}