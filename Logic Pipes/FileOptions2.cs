using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOptions2
{
    class OptionSet
    {
        private class Entry
        {
            public string name, value;
            public Entry(string name, string value = null) { this.name = name; this.value = value; }
        }
        string Path;
        List<Entry> Set = new List<Entry>();
        bool SetContains(string name)
        {
            foreach (Entry o in Set)
                if (o.name == name) return true;
            return false;
        }
        string[] ReadFile() { return File.ReadAllLines(Path); }
        void Reload()
        {
            foreach (string line in ReadFile())
            {
                if (!(line.StartsWith(";") || string.IsNullOrEmpty(line)))
                {
                    string[] split = line.Split('=');
                    if (!SetContains(split[0]))
                    {
                        if (split.Length == 2 || string.IsNullOrEmpty(split[1])) Set.Add(new Entry(split[0], split[1]));
                        else Set.Add(new Entry(split[0]));
                    }
                }
            }
        }
        public OptionSet(string path, bool makefile = true)
        {
            Path = path;
            if (!File.Exists(path))
            {
                if (makefile) File.WriteAllText(path, null);
                else throw new ArgumentException("No file found");
            }
            else Reload();
        }
        public void NewEntry(string name, string value = null)
        {
            if (!SetContains(name))
            {
                File.AppendAllText(Path, name + "=" + value + Environment.NewLine);
                Reload();
            }
            else throw new ArgumentException("Option already exists");
        }
        public bool SafeNewEntry(string name, string value = null)
        {
            try { NewEntry(name, value); return true; } catch (ArgumentException) { return false; }
        }
        public void EditEntryValue(string name, string newvalue)
        {
            if (SetContains(name))
            {
                string[] temps = ReadFile();
                for (int place = 0; place < temps.Length; place++)
                    if (temps[place].StartsWith(name + "="))
                    {
                        temps[place] = name + "=" + newvalue;
                        Reload();
                        return;
                    }
            }
            else throw new ArgumentException("Entry not found");
        }
        public bool SafeEditEntryValue(string name, string newvalue)
        {
            try { EditEntryValue(name, newvalue); return true; } catch (ArgumentException) { return false; }
        }
        public void DeleteEntry(string name)
        {
            string[] temps = ReadFile();
            List<string> toset = new List<string>();
            foreach (string line in temps)
                if (!line.StartsWith(name + "=")) toset.Add(line);
            File.WriteAllLines(Path, toset);
            Reload();
        }
        public bool SafeDeleteEntry(string name)
        {
            try { DeleteEntry(name); return true; } catch (ArgumentException) { return false; }
        }
        public string GetEntryValue(string name)
        {
            foreach (Entry e in Set)
                if (e.name == name) return e.value;
            throw new ArgumentException("No entry under that name");
        }
        public bool SafeGetEntryValue(string name, out string value)
        {
            try { value = GetEntryValue(name); return true; } catch { value = null; return false; }
        }
        public bool CompareValue(string name, string compare)
        {
            return GetEntryValue(name) == compare;
        }
        public bool CompareValues(string compare, params string[] names)
        {
            foreach (string name in names)
                if (!CompareValue(name, compare)) return false;
            return true;
        }
        public bool CompareEntryValues(params string[] names)
        {
            if (names.Length == 1) return true;
            for (int place = 1; place < names.Length; place++)
                if (!CompareValue(names[place-1], GetEntryValue(names[place-1]))) return false;
            return true;
        }
        public string[] GetEntryNamesWithValue(string value)
        {
            List<string> toreturn = new List<string>();
            foreach (Entry e in Set)
                if (e.value == value) toreturn.Add(e.name);
            return toreturn.ToArray();
        }
        public int NamesWithValueLength(string value)
        {
            return GetEntryNamesWithValue(value).Length;
        }
        public bool NamesWithValueContains(string value, string name)
        {
            foreach (string v in GetEntryNamesWithValue(value))
                if (v == name) return true;
            return false;
        }
        public bool EntryExists(string name)
        {
            return SetContains(name);
        }
        public void getAllEntryNamesAndValues(out string[] names, out string[] values)
        {
            List<string> n = new List<string>();
            List<string> v = new List<string>();
            foreach(Entry e in Set) { n.Add(e.name); v.Add(e.value); }
            names = n.ToArray(); values = v.ToArray();
        }
    }
}
