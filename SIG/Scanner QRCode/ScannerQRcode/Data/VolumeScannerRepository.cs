using ScannerQRcode.Models;
using SQLite;
using System.Linq;
using static SQLite.TableMapping;

namespace ScannerQRcode.Data
{
    public class VolumeScannerRepository : IDisposable
    {
        private readonly SQLiteConnection database;
        public VolumeScannerRepository(string dbName) 
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dbName);
            database = new SQLiteConnection(dbPath);
            database.CreateTable<VolumeScanner>();
            database.CreateTable<VolumeLookup>();
        }

        public List<VolumeScanner> GetVolumeScanners()
        {
            //return database.Table<VolumeScanner>().ToList();
            return database.Query<VolumeScanner>("SELECT Volume FROM VolumeScanner GROUP BY Volume");
        }

        public int CreateVolumeScanner(VolumeScanner account)
        {
            return database.Insert(account);
        }

        public int UpdateVolumeScanner(VolumeScanner account)
        {
            return database.Update(account);
        }

        public int DeleteVolumeScanner(VolumeScanner account)
        {
            return database.Delete(account);
        }

        public List<VolumeLookup> QueryVolumeLookup(string volume)
        {
            return database.Query<VolumeLookup>("SELECT * FROM VolumeLookup WHERE Volume == " + volume);
        }

        public VolumeLookup GetVolumeLookup(string volume)
        {
            return database.Table<VolumeLookup>().Where(a => a.Volume == volume).FirstOrDefault();
        }

        public void Dispose()
        {
            database.Dispose();
        }

    }
}
