using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RefactoringTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var fileStore = new FileStore(Directory.GetCurrentDirectory());

            fileStore.Save(1, "testing");

            Console.WriteLine(fileStore.Read(1).DefaultIfEmpty("").Single());
            Console.ReadLine();
        }
    }

    public class FileStore
    {
        public FileStore(string workingDirectory)
        {
            if(workingDirectory == null)
            {
                throw new ArgumentNullException("workingDirectory");
            }
            if(!Directory.Exists(workingDirectory))
            {
                throw new Exception("Invalid path for working directory");
            }
            WorkingDirectory = workingDirectory;
        }
        public string WorkingDirectory { get; private set; }

        public void Save(int id, string message)
        {
            var path = GetPathForId(id);
            File.WriteAllText(path, message);
        }

        public Maybe<string> Read(int id)
        {
            var path = GetPathForId(id);
            if (!File.Exists(path))
            {
                return new Maybe<string>();
            }
            
            var message = File.ReadAllText(path);
            return new Maybe<string>(message);
        }

        public string GetPathForId(int id)
        {
            return Path.Combine(WorkingDirectory, id + ".txt");
        }
    }

    public class Maybe<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _values;

        public Maybe()
        {
            _values = new T[0];
        }

        public Maybe(T value)
        {
            _values = new[] { value };
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}