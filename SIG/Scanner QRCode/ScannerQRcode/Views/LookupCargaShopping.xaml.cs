using ScannerQRcode.Data;
using ScannerQRcode.Data.Api.Models;
using ScannerQRcode.Models;
using ScannerQRcode.ViewModels;
using System.Collections.ObjectModel;
using Telerik.Maui.Controls;

namespace ScannerQRcode.Views;

public partial class LookupCargaShopping : ContentPage
{

    private readonly VolumeScannerRepository _volumeScannerRepository;

    public LookupCargaShopping(VolumeScannerRepository volumeScannerRepository, LookupCargaShoppingViewModel vm)
	{
        _volumeScannerRepository = volumeScannerRepository;

        InitializeComponent();
        BindingContext = vm;

        CarregarAprovados();
        //var list = GetResults();

    }

    private async void CarregarAprovados()
    {
        LookupCargaShoppingViewModel vm = (LookupCargaShoppingViewModel)BindingContext;
        try
        {
            vm.Status = "Buscando Siglas aprovadas.";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://mail.cipolatti.com.br:8080/api/Aprovado/SelecionarTodos");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                vm.Aprovados = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Aprovado>>(content);

                var lookup = await _volumeScannerRepository.GetLookupCarregamento();

                if (lookup != null)
                {
                    foreach (var item in lookup.Sigla.Split(',').ToList())
                        AcSigla.Tokens.Add(vm.Aprovados.Where(x => x.SiglaServ == item).FirstOrDefault());

                    vm.Caminhao = lookup.Caminhao;
                    vm.PlacaCaminhao = lookup.PlacaCaminhao;
                    vm.BloqCaminao = AcSigla.Tokens.Count <= 0;
                }

                vm.IsLoading = false;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro ao carregar Siglas", ex.Message, "OK");
            vm.IsLoading = false;
        }
    }

    private void radAutoCompleteViewSigla_SuggestionItemSelected(object sender, Telerik.Maui.Controls.AutoComplete.SuggestionItemSelectedEventArgs e)
    {
        var item = sender as RadAutoComplete;
        LookupCargaShoppingViewModel vm = (LookupCargaShoppingViewModel)BindingContext;
        vm.BloqCaminao = item.Tokens.Count <= 0;
        vm.Caminhao = item.Tokens.Count > 0 ? "" : vm.Caminhao;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        LookupCargaShoppingViewModel vm = (LookupCargaShoppingViewModel)BindingContext;
        vm.IsLoading = true;
        //var volumes = await _volumeScannerRepository.QueryAllVolumeLookup();
        if (AcSigla.Tokens.Count == 0)
        {
            vm.IsLoading = false;
            await DisplayAlert("Informação ausente", "Precisa selecionar sigla(s) para baixar volume(s)", "OK");
            return;
        }
        else if (vm.PlacaCaminhao == null || vm.PlacaCaminhao.Length == 0)
        {
            vm.IsLoading = false;
            await DisplayAlert("Informação ausente", "Precisa informar a placa do veículo", "OK");
            return;
        }
        await _volumeScannerRepository.DeleteVolumeLookup();
        foreach (Aprovado item in AcSigla.Tokens.Cast<Aprovado>())
        {
            if(vm.Caminhao == null || vm.Caminhao.Length == 0)
            {
                //await DisplayAlert("Lookup", $"SIGLA: {item.SiglaServ}", "OK");
                vm.Status = $"Buscando volumes da sigla {item.SiglaServ}";
                ObservableCollection<Lookup> lookups = await GetLookupBySigla(item.SiglaServ);
                Dispatcher.Dispatch(async () =>
                {
                    foreach (var lookup in lookups)
                    {
                        await _volumeScannerRepository.CreateVolumeLookup(new VolumeLookup { Volume = lookup.Barcode, Qrcode = lookup.Qrcode });
                    }
                });
            }
            else
            {
                string[] caminhoes = vm.Caminhao.Split(';');
                foreach (var caminhao in caminhoes)
                {
                    //await DisplayAlert("Lookup", $"SIGLA: {item.SiglaServ} | CAMINHÃO {caminhao}", "OK");
                    vm.Status = $"Buscando volumes da sigla {item.SiglaServ}";
                    ObservableCollection<Lookup> lookups = await GetLookupBySiglaByCaminhao(item.SiglaServ, caminhao);
                    Dispatcher.Dispatch(async () =>
                    {
                        foreach (var lookup in lookups)
                        {
                            await _volumeScannerRepository.CreateVolumeLookup(new VolumeLookup { Volume = lookup.Barcode, Qrcode = lookup.Qrcode });
                        }
                    });
                }
            }
        }
        vm.IsLoading = false;

        //foreach (var item in BusyIndicator)

        await _volumeScannerRepository.CreateLookupCarregamento(
            new LookupCarregamento() 
            { 
                Sigla = string.Join(",", AcSigla.Tokens.Cast<Aprovado>().Select(x => x.SiglaServ)), 
                Caminhao = EtCaminhao.Text, 
                PlacaCaminhao = EtPlacaCaminao.Text
            });
        await Shell.Current.GoToAsync(nameof(ReaderCargaShopping));
    }

    private async Task<ObservableCollection<Lookup>> GetLookupBySigla(string sigla)
    {
        LookupCargaShoppingViewModel vm = (LookupCargaShoppingViewModel)BindingContext;
        try
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"http://mail.cipolatti.com.br:8080/api/Lookup/LookupBySigla?sigla={sigla}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                vm.Lookups = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Lookup>>(content);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro ao carregar Siglas", ex.Message, "OK");
            vm.IsLoading = false;
        }
        return vm.Lookups;
    }

    private async Task<ObservableCollection<Lookup>> GetLookupBySiglaByCaminhao(string sigla, string caminhao)
    {
        LookupCargaShoppingViewModel vm = (LookupCargaShoppingViewModel)BindingContext;
        try
        {
            vm.IsLoading = true;
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"http://mail.cipolatti.com.br:8080/api/Lookup/lookupBySiglaByCaminhao?sigla={sigla}&caminhao={caminhao}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                vm.Lookups = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Lookup>>(content);
                vm.IsLoading = false;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro ao carregar Siglas", ex.Message, "OK");
            vm.IsLoading = false;
        }
        return vm.Lookups;
    }
}