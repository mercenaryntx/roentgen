namespace Neurotoxin.Roentgen.Data.Entities
{
    public abstract class FileEntityBase : EntityBase 
    {
        private string _path;

        public string Path
        {
            get => _path;
            set { _path = value;
                Name = System.IO.Path.GetFileName(value);
            }
        }
    }
}