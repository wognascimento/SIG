using MLKit.BarcodeScanning;
using ScannerQRcode.Models;
using SQLite;

namespace ScannerQRcode.Data
{
    public class VolumeScannerRepository : IDisposable
    {
        //private readonly SQLiteConnection database;
        SQLiteAsyncConnection database;
        public VolumeScannerRepository(string dbName) 
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dbName);
            //database = new SQLiteConnection(dbPath);
            //database.CreateTable<VolumeScanner>();
            //database.CreateTable<VolumeLookup>();
        }

        async Task Init()
        {
            if (database is not null)
                return;

            database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            //var result = await database.CreateTableAsync<TodoItem>();
            await database.CreateTableAsync<VolumeScanner>();
            await database.CreateTableAsync<VolumeLookup>();
            await database.CreateTableAsync<LookupCarregamento>();
            await database.CreateTableAsync<VolumeEnderecamento>();
            await database.CreateTableAsync<EnderecoGalpao>();
        }

        public async Task<int> CreateLookupCarregamento(LookupCarregamento lookupCarregamento)
        {
            await Init();
            return await database.InsertAsync(lookupCarregamento);
        }

        public async Task<LookupCarregamento> GetLookupCarregamento()
        {
            await Init();
            return await database.Table<LookupCarregamento>().FirstOrDefaultAsync();
        }

        public async Task<List<VolumeScanner>> GetVolumeScanners()
        {
            //return database.Table<VolumeScanner>().ToList();
            //return database.Query<VolumeScanner>("SELECT Volume FROM VolumeScanner GROUP BY Volume");
            await Init();
            return await database.Table<VolumeScanner>().ToListAsync();
        }

        public async Task<int> CreateVolumeScanner(VolumeScanner volumeScanner)
        {
            //return database.Insert(account);
            await Init();
            return await database.InsertAsync(volumeScanner);
        }

        public async Task<int> UpdateVolumeScanner(VolumeScanner volumeScanner)
        {
            //return database.Update(account);
            await Init();
            return await database.UpdateAsync(volumeScanner);
        }

        public async Task<int> DeleteVolumeScanner(VolumeScanner volumeScanner)
        {
            //return database.Delete(account);
            await Init();
            return await database.DeleteAsync(volumeScanner);
        }

        public async Task<List<VolumeLookup>> QueryAllVolumeLookup()
        {
            //return database.Query<VolumeLookup>("SELECT * FROM VolumeLookup");
            await Init();
            return await database.Table<VolumeLookup>().ToListAsync();
        }

        public async Task<List<VolumeLookup>> QueryVolumeLookup(string volume)
        {
            //return database.Query<VolumeLookup>("SELECT * FROM VolumeLookup WHERE Volume == " + volume);
            await Init();
            return await database.Table<VolumeLookup>().Where(i => i.Volume == volume).ToListAsync();
        }

        public async Task<VolumeLookup> GetVolumeLookupByQrCode(string qrcode)
        {
            await Init();
            return await database.Table<VolumeLookup>().Where(i => i.Qrcode == qrcode).FirstOrDefaultAsync();
        }

        public async Task<VolumeLookup> GetVolumeLookupByCode39(string code39)
        {
            await Init();
            return await database.Table<VolumeLookup>().Where(i => i.Volume == code39).FirstOrDefaultAsync();
        }

        public async Task<int> CreateVolumeLookup(VolumeLookup lookup)
        {
            //return database.Insert(lookup);
            await Init();
            return await database.InsertAsync(lookup);
        }

        public async Task<int> DeleteVolumeLookup()
        {
            //return database.DeleteAll<VolumeLookup>();
            await Init();
            return await database.DeleteAllAsync<VolumeLookup>();
        }

        public async Task<int> CreateVolumeEnderecamento(VolumeEnderecamento enderecamento)
        {
            await Init();
            return await database.InsertAsync(enderecamento);
        }

        public async Task<List<VolumeEnderecamento>> QueryAllVolumeEnderecados()
        {
            await Init();
            return await database.Table<VolumeEnderecamento>().ToListAsync();
        }

        public async Task<int> DeleteAllVolumeEnderecados()
        {
            await Init();
            return await database.DeleteAllAsync<VolumeEnderecamento>();
        }

        public async Task<int> CreateEndereco(EnderecoGalpao endereco)
        {
            await Init();
            return await database.InsertAsync(endereco);
        }

        public async Task<List<EnderecoGalpao>> QueryAllEnderecos()
        {
            await Init();
            return await database.Table<EnderecoGalpao>().ToListAsync();
        }

        public async Task<int> DeleteAllEnderecos()
        {
            await Init();
            return await database.DeleteAllAsync<EnderecoGalpao>();
        }

        public async Task<EnderecoGalpao> GetEndereco(string Barcode)
        {
            await Init();
            return await database.Table<EnderecoGalpao>().Where(i => i.Barcode == Barcode).FirstOrDefaultAsync();
        }

        public void Dispose()
        {
            //database.Dispose();
        }

    }

    public static class Constants
    {
        public const string DatabaseFilename = "ColetorSQLite.db3"; //coletor.db

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
    }
}
