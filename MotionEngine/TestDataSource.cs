using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MotionEngine
{
    public class TestDataSource
    {
        private List<TimeStampedObjects> _objectTimeline;
        private bool _reverse = false;
        private int _currentIndex = 0;

        public TestDataSource()
        {
            LoadJsonData("Data/data.json");
        }

        // Load JSON data into the list
        public async Task LoadJsonData(string filePath)
        {

            string directoryPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            // Combine the directory path with the filename
            string jsonFilePath = Path.Combine(directoryPath, filePath);
            if (File.Exists(jsonFilePath))
            {
                string json = File.ReadAllText(jsonFilePath);
                _objectTimeline = JsonSerializer.Deserialize<List<TimeStampedObjects>>(json);
            }
            else
            {
                Console.WriteLine("File not found.");
            }
        }
        // Get the current object in the list
        public TimeStampedObjects GetCurrent()
        {
            if (_objectTimeline == null || _objectTimeline.Count == 0)
            {
                throw new InvalidOperationException("No data loaded.");
            }

            return _objectTimeline[_currentIndex];
        }

        // Move to the next object in the timeline
        public TimeStampedObjects MoveNext()
        {
            if (_reverse)
            {
                _currentIndex--;
                if (_currentIndex < 0)
                {
                    _currentIndex = 1;
                    _reverse = false;
                }
            }
            else
            {
                _currentIndex++;
                if (_currentIndex >= _objectTimeline.Count)
                {
                    _currentIndex = _objectTimeline.Count - 2;
                    _reverse = true;
                }
            }

            return GetCurrent();
        }
    }


}
