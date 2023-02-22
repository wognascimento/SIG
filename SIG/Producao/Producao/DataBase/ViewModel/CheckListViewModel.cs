using Microsoft.EntityFrameworkCore;
using Npgsql;
using Producao.Views;
using Producao.Views.PopUp;
using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Producao
{
    public class CheckListViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<SiglaChkListModel> _siglas;
        public ObservableCollection<SiglaChkListModel> Siglas
        {
            get { return _siglas; }
            set
            {
                _siglas = value;
                RaisePropertyChanged("Siglas");
            }
        }
        private SiglaChkListModel _sigla;
        public SiglaChkListModel Sigla
        {
            get { return _sigla; }
            set
            {
                _sigla = value;
                RaisePropertyChanged("Sigla");
            }
        }
        private ObservableCollection<RelplanModel> _planilhas;
        public ObservableCollection<RelplanModel> Planilhas
        {
            get { return _planilhas; }
            set
            {
                _planilhas = value;
                RaisePropertyChanged("Planilhas");
            }
        }
        private RelplanModel _planilha;
        public RelplanModel Planilha
        {
            get { return _planilha; }
            set
            {
                _planilha = value;
                RaisePropertyChanged("Planilha");
            }
        }
        private ObservableCollection<ProdutoModel> _produtos;
        public ObservableCollection<ProdutoModel> Produtos
        {
            get { return _produtos; }
            set
            {
                _produtos = value;
                RaisePropertyChanged("Produtos");
            }
        }
        private ProdutoModel _produto;
        public ProdutoModel Produto
        {
            get { return _produto; }
            set
            {
                _produto = value;
                RaisePropertyChanged("Produto");
            }
        }

        private ObservableCollection<TabelaDescAdicionalModel> _descAdicionais;
        public ObservableCollection<TabelaDescAdicionalModel> DescAdicionais
        {
            get { return _descAdicionais; }
            set
            {
                _descAdicionais = value;
                RaisePropertyChanged("DescAdicionais");
            }
        }
        private TabelaDescAdicionalModel _descAdicional;
        public TabelaDescAdicionalModel DescAdicional
        {
            get { return _descAdicional; }
            set
            {
                _descAdicional = value;
                RaisePropertyChanged("DescAdicional");
            }
        }

        private ObservableCollection<TblComplementoAdicionalModel> _compleAdicionais;
        public ObservableCollection<TblComplementoAdicionalModel> CompleAdicionais
        {
            get { return _compleAdicionais; }
            set
            {
                _compleAdicionais = value;
                RaisePropertyChanged("CompleAdicionais");
            }
        }
        private TblComplementoAdicionalModel _compledicional;
        public TblComplementoAdicionalModel Compledicional
        {
            get { return _compledicional; }
            set
            {
                _compledicional = value;
                RaisePropertyChanged("Compledicional");
            }
        }

        private ObservableCollection<ComplementoCheckListModel> _complementoCheckLists;
        public ObservableCollection<ComplementoCheckListModel> ComplementoCheckLists
        {
            get { return _complementoCheckLists; }
            set
            {
                _complementoCheckLists = value;
                RaisePropertyChanged("ComplementoCheckLists");
            }
        }

        private ComplementoCheckListModel complementoCheckList;
        public ComplementoCheckListModel ComplementoCheckList
        {
            get { return complementoCheckList; }
            set
            {
                complementoCheckList = value;
                RaisePropertyChanged("ComplementoCheckList");
            }
        }
        private ObservableCollection<object> _locaisshopping;
        public ObservableCollection<object> Locaisshopping
        {
            get { return _locaisshopping; }
            set
            {
                _locaisshopping = value;
                RaisePropertyChanged("Locaisshopping");
            }
        }

        private QryCheckListGeralModel _checkListGeral;
        public QryCheckListGeralModel CheckListGeral
        {
            get { return _checkListGeral; }
            set
            {
                _checkListGeral = value;
                RaisePropertyChanged("CheckListGeral");
            }
        }
        private ObservableCollection<QryCheckListGeralModel> _checkListGerais;
        public ObservableCollection<QryCheckListGeralModel> CheckListGerais
        {
            get { return _checkListGerais; }
            set
            {
                _checkListGerais = value;
                RaisePropertyChanged("CheckListGerais");
            }
        }

        private DetalhesComplemento _detCompl;
        public DetalhesComplemento DetCompl
        {
            get { return _detCompl; }
            set
            {
                _detCompl = value;
                RaisePropertyChanged("DetCompl");
            }
        }
        private ObservableCollection<DetalhesComplemento> _detCompls;
        public ObservableCollection<DetalhesComplemento> DetCompls
        {
            get { return _detCompls; }
            set
            {
                _detCompls = value;
                RaisePropertyChanged("DetCompls");
            }
        }

        private QryCheckListGeralComplementoModel _checkListGeralComplemento;
        public QryCheckListGeralComplementoModel CheckListGeralComplemento
        {
            get { return _checkListGeralComplemento; }
            set
            {
                _checkListGeralComplemento = value;
                RaisePropertyChanged("CheckListGeralComplemento");
            }
        }
        private ObservableCollection<QryCheckListGeralComplementoModel> _checkListGeralComplementos;
        public ObservableCollection<QryCheckListGeralComplementoModel> CheckListGeralComplementos
        {
            get { return _checkListGeralComplementos; }
            set
            {
                _checkListGeralComplementos = value;
                RaisePropertyChanged("CheckListGeralComplementos");
            }
        }

        private SetorProducaoModel _setorProducao;
        public SetorProducaoModel SetorProducao
        {
            get { return _setorProducao; }
            set
            {
                _setorProducao = value;
                RaisePropertyChanged("SetorProducao");
            }
        }
        private ObservableCollection<SetorProducaoModel> _setoresProducao;
        public ObservableCollection<SetorProducaoModel> SetoresProducao
        {
            get { return _setoresProducao; }
            set
            {
                _setoresProducao = value;
                RaisePropertyChanged("SetoresProducao");
            }
        }


        /**/
        private ProdutoOsModel _produtoOs;
        public ProdutoOsModel ProdutoOs
        {
            get { return _produtoOs; }
            set
            {
                _produtoOs = value;
                RaisePropertyChanged("ProdutoOs");
            }
        }
        private ObservableCollection<ProdutoOsModel> _produtoOss;
        public ObservableCollection<ProdutoOsModel> ProdutoOss
        {
            get { return _produtoOss; }
            set
            {
                _produtoOss = value;
                RaisePropertyChanged("ProdutoOss");
            }
        }

        /**/
        private ProdutoServicoModel _produtoServico;
        public ProdutoServicoModel ProdutoServico
        {
            get { return _produtoServico; }
            set
            {
                _produtoServico = value;
                RaisePropertyChanged("ProdutoServico");
            }
        }
        private ObservableCollection<ProdutoServicoModel> _produtoServicos;
        public ObservableCollection<ProdutoServicoModel> ProdutoServicos
        {
            get { return _produtoServicos; }
            set
            {
                _produtoServicos = value;
                RaisePropertyChanged("ProdutoServicos");
            }
        }

        /**/
        private RequisicaoModel _requisicao;
        public RequisicaoModel Requisicao
        {
            get { return _requisicao; }
            set
            {
                _requisicao = value;
                RaisePropertyChanged("Requisicao");
            }
        }
        private ObservableCollection<RequisicaoModel> _requisicoes;
        public ObservableCollection<RequisicaoModel> Requisicoes
        {
            get { return _requisicoes; }
            set
            {
                _requisicoes = value;
                RaisePropertyChanged("Requisicoes");
            }
        }

        /**/
        private DetalheRequisicaoModel _requisicaoDetalhe;
        public DetalheRequisicaoModel RequisicaoDetalhe
        {
            get { return _requisicaoDetalhe; }
            set
            {
                _requisicaoDetalhe = value;
                RaisePropertyChanged("RequisicaoDetalhe");
            }
        }
        private ObservableCollection<DetalheRequisicaoModel> _requisicaoDetalhes;
        public ObservableCollection<DetalheRequisicaoModel> RequisicaoDetalhes
        {
            get { return _requisicaoDetalhes; }
            set
            {
                _requisicaoDetalhes = value;
                RaisePropertyChanged("RequisicaoDetalhes");
            }
        }

        private QryRequisicaoDetalheModel _qryRequisicaoDetalhe;
        public QryRequisicaoDetalheModel QryRequisicaoDetalhe
        {
            get { return _qryRequisicaoDetalhe; }
            set
            {
                _qryRequisicaoDetalhe = value;
                RaisePropertyChanged("QryRequisicaoDetalhe");
            }
        }
        private ObservableCollection<QryRequisicaoDetalheModel> _qryRequisicaoDetalhes;
        public ObservableCollection<QryRequisicaoDetalheModel> QryRequisicaoDetalhes
        {
            get { return _qryRequisicaoDetalhes; }
            set
            {
                _qryRequisicaoDetalhes = value;
                RaisePropertyChanged("QryRequisicaoDetalhes");
            }
        }


        private IList _chkGeralRelatorios;
        public IList ChkGeralRelatorios
        {
            get { return _chkGeralRelatorios; }
            set
            {
                _chkGeralRelatorios = value;
                RaisePropertyChanged("ChkGeralRelatorios");
            }
        }

        private ICommand rowDataCommand { get; set; }
        public ICommand RowDataCommand
        {
            get
            {
                return rowDataCommand;
            }
            set
            {
                rowDataCommand = value;
            }
        }


        public CheckListViewModel()
        {
            DetCompl = new DetalhesComplemento();
            ComplementoCheckList = new ComplementoCheckListModel();
            Siglas = new ObservableCollection<SiglaChkListModel>();
            Planilhas = new ObservableCollection<RelplanModel>();

            Task.Run(async () => { Siglas = await GetSiglasAsync(); });
            Task.Run(async () => { Planilhas = await GetPlanilhasAsync(); });

            rowDataCommand = new RelayCommand(ChangeCanExecute);
        }
        public async Task GetSetoresAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.SetorProducaos.OrderBy(c => c.setor).Where(c => c.inativo.Equals("0")).ToListAsync();
                SetoresProducao = new ObservableCollection<SetorProducaoModel>(data);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async void ChangeCanExecute(object obj)
        {
            try
            {
                await Task.Run(async () => await GetSetoresAsync());

                var window = new Window();
                var stackPanel = new StackPanel { Orientation = Orientation.Vertical };
                SfMultiColumnDropDownControl sfMultiColumn = new SfMultiColumnDropDownControl();
                sfMultiColumn.Height= 38;
                sfMultiColumn.AllowAutoComplete = true;
                sfMultiColumn.IsDropDownOpen = true;
                sfMultiColumn.AutoGenerateColumns = false;
                sfMultiColumn.GridColumnSizer = GridLengthUnitType.AutoLastColumnFill;
                sfMultiColumn.SearchCondition = SearchCondition.Contains;//  "Contains";
                sfMultiColumn.DisplayMember = "setor";
                sfMultiColumn.ValueMember = "codigo_setor";
                sfMultiColumn.Columns.Add(new GridTextColumn() { MappingName = "setor" });
                sfMultiColumn.Columns.Add(new GridTextColumn() { MappingName = "localizacao" });
                sfMultiColumn.Columns.Add(new GridTextColumn() { MappingName = "galpao" });
                sfMultiColumn.ItemsSource = SetoresProducao;
                stackPanel.Children.Add(sfMultiColumn);
                Button btn = new Button();
                btn.Content = "OK";
                btn.Click += async (s, e) =>
                {
                    
                    try
                    {
                        SetorProducao = (SetorProducaoModel)sfMultiColumn.SelectedItem;

                        ProdutoOs = new ProdutoOsModel
                        {
                            tipo = "PEÇA NOVA",
                            planilha = CheckListGeral.planilha,
                            cod_produto = CheckListGeral.codigo,
                            cod_desc_adicional = CheckListGeral.coduniadicional,
                            cod_compl_adicional = CheckListGeralComplemento.codcompladicional,
                            quantidade = 0,
                            data_emissao = DateTime.Now,
                            responsavel_emissao = Environment.UserName,
                            solicitado_por = Environment.UserName
                        };

                        await Task.Run(async () => await CriarOsProdutoAsync());
                        ProdutoServico = new ProdutoServicoModel
                        {
                            num_os_produto = ProdutoOs.num_os_produto,
                            tipo = ProdutoOs.tipo,
                            codigo_setor = SetorProducao.codigo_setor,
                            setor_caminho = $"{SetorProducao.setor} - {SetorProducao.galpao}",
                            quantidade = ProdutoOs.quantidade,
                            data_inicio = DateTime.Now,
                            data_fim = DateTime.Now.AddDays(1),
                            cliente = Sigla.sigla_serv,
                            tema = Sigla.tema,
                            orientacao_caminho = "OS DESTINADA A REQUISIÇÃO DE MATERIAL PARA A PLANILHA",
                            codigo_setor_proximo = 39,    
                            setor_caminho_proximo = "FINAL - TODOS",
                            fase = "PRODUÇÃO",
                            responsavel_emissao_os = Environment.UserName,
                            emitida_por = Environment.UserName,
                            emitida_data = DateTime.Now,
                            retrabalho = "NÃO",
                            impresso = "-1",
                            cod_detalhe_compl = CheckListGeralComplemento.coddetalhescompl
                        };

                        await Task.Run(async () => await CriarProdutoServicoAsync());

                        Requisicao = new RequisicaoModel
                        {
                            num_os_servico = ProdutoServico.num_os_servico,
                            data = DateTime.Now,
                            alterado_por = Environment.UserName

                        };

                        await Task.Run(async () => await CriarRequisicaoAsync());
              
                        window.Close();

                        RequisicaoMaterial detailsWindow = new RequisicaoMaterial(ProdutoServico);
                        detailsWindow.Owner = Window.GetWindow((DependencyObject)obj); //Window.GetWindow((DependencyObject)obj)
                        detailsWindow.ShowDialog();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    
                    

                };

                await Task.Run(async () => await GetProdutoServicoAsync());

                if (ProdutoServico == null)
                {
                    stackPanel.Children.Add(btn);
                    window.Content = stackPanel;
                    window.Title = "Criar Requisição";
                    window.Height = 100;
                    window.Width = 350;
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.WindowStyle = WindowStyle.ToolWindow;
                    window.ResizeMode = ResizeMode.NoResize;
                    //window.Owner = (Window)obj;
                    window.ShowDialog();
                }
                else
                {
                    await Task.Run(async () => await GetRequisicaoAsync());
                    await Task.Run(async () => await GetRequisicaoDetalhesAsync());
                    RequisicaoMaterial detailsWindow = new RequisicaoMaterial(ProdutoServico); //ProdutoServico
                    detailsWindow.Owner = Window.GetWindow((DependencyObject)obj);  //(Window)obj;
                    detailsWindow.ShowDialog();
                }
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        public async Task CriarOsProdutoAsync()
        {
            try
            {
                using DatabaseContext db = new();
                db.Entry(ProdutoOs).State = ProdutoOs.num_os_produto == null ?
                                   EntityState.Added :
                                   EntityState.Modified;

                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task CriarProdutoServicoAsync()
        {
            try
            {
                using DatabaseContext db = new();
                db.Entry(ProdutoServico).State = ProdutoServico.num_os_servico == null ?
                                   EntityState.Added :
                                   EntityState.Modified;

                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task GetProdutoServicoAsync()
        {
            try
            {
                using DatabaseContext db = new();
                ProdutoServico = await db.ProdutoServicos.Where(p => p.cod_detalhe_compl == CheckListGeralComplemento.coddetalhescompl).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task CriarRequisicaoAsync()
        {
            try
            {
                using DatabaseContext db = new();
                db.Entry(Requisicao).State = Requisicao.num_requisicao == null ?
                                   EntityState.Added :
                                   EntityState.Modified;

                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task GetRequisicaoAsync()
        {
            try
            {
                using DatabaseContext db = new();
                Requisicao = await db.Requisicoes.Where(r => r.num_os_servico == ProdutoServico.num_os_servico).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task GetRequisicaoDetalhesAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.QryRequisicaoDetalhes.Where(r => r.num_requisicao == Requisicao.num_requisicao).ToListAsync();
                QryRequisicaoDetalhes = new ObservableCollection<QryRequisicaoDetalheModel>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<SiglaChkListModel>> GetSiglasAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.Siglas.OrderBy(c => c.sigla_serv).ToListAsync();
                return new ObservableCollection<SiglaChkListModel>(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return new ObservableCollection<SiglaChkListModel>();
        }
        public async Task<ObservableCollection<RelplanModel>> GetPlanilhasAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.Relplans.OrderBy(c => c.planilha).Where(c => c.ativo.Equals("1")).ToListAsync();
                return new ObservableCollection<RelplanModel>(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return new ObservableCollection<RelplanModel>();
        }
        public async Task<ObservableCollection<ProdutoModel>> GetProdutosAsync()
        {
            try
            {
                Produtos = new ObservableCollection<ProdutoModel>();
                using DatabaseContext db = new();
                var data = await db.Produtos
                    .OrderBy(c => c.descricao)
                    .Where(c => c.planilha.Equals(Planilha.planilha))
                    .Where(c => c.inativo != "-1")
                    .ToListAsync();
                Produtos = new ObservableCollection<ProdutoModel>(data);
                return Produtos;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return Produtos;
        }
        public async Task<ObservableCollection<TabelaDescAdicionalModel>> GetDescAdicionaisAsync()
        {
            try
            {
                DescAdicionais = new ObservableCollection<TabelaDescAdicionalModel>();
                using DatabaseContext db = new();
                var data = await db.DescAdicionais
                    .OrderBy(c => c.descricao_adicional)
                    .Where(c => c.codigoproduto.Equals(Produto.codigo))
                    .Where(c => c.inativo != "-1")
                    .ToListAsync();

                DescAdicionais = new ObservableCollection<TabelaDescAdicionalModel>(data);
                return DescAdicionais;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return DescAdicionais;
        }

        public async Task<ObservableCollection<TblComplementoAdicionalModel>> GetCompleAdicionaisAsync()
        {
            //(long)((CheckListViewModel)DataContext).ComplementoCheckList.coduniadicional
            try
            {
                CompleAdicionais = new ObservableCollection<TblComplementoAdicionalModel>();
                using DatabaseContext db = new();
                var data = await db.ComplementoAdicionais
                    .OrderBy(c => c.complementoadicional)
                    .Where(c => c.coduniadicional.Equals(ComplementoCheckList.coduniadicional))
                    .Where(c => c.inativo != "-1")
                    .ToListAsync();

                CompleAdicionais = new ObservableCollection<TblComplementoAdicionalModel>(data);
                //return new ObservableCollection<TblComplementoAdicionalModel>(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //return new ObservableCollection<TblComplementoAdicionalModel>();
            return CompleAdicionais;
        }

        public async Task<ObservableCollection<object>> GetLocaisShoppAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.ComplementoCheckLists
                    .OrderBy(c => c.local_shoppings)
                    .Where(c => c.id_aprovado == Sigla.id_aprovado)
                    .Select(s => new
                    {
                        s.local_shoppings
                    })
                    .ToArrayAsync();
                Locaisshopping = new ObservableCollection<object>(data.GroupBy(x => x.local_shoppings));
                return Locaisshopping;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return Locaisshopping;
        }

        public async Task<ObservableCollection<QryCheckListGeralModel>> GetCheckListGeralAsync()
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.CheckListGerals
                    .OrderBy(c => c.id)
                    .Where(c => c.id_aprovado == Sigla.id_aprovado)
                    .ToListAsync();
                CheckListGerais = new ObservableCollection<QryCheckListGeralModel>(data);
                return CheckListGerais;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return CheckListGerais;
        }

        public async Task<ComplementoCheckListModel> AddComplementoCheckListAsync()
        {
            if (ComplementoCheckList == null)
            {
                throw new ArgumentNullException($"{nameof(AddComplementoCheckListAsync)} entity must not be null");
            }

            try
            {
                using DatabaseContext db = new();
                db.Entry(ComplementoCheckList).State = ComplementoCheckList.codcompl == null ?
                                   EntityState.Added :
                                   EntityState.Modified;

                await db.SaveChangesAsync();

                return ComplementoCheckList;
            }
            catch (NpgsqlException)
            {
                throw;
            }
        }

        public async Task<QryCheckListGeralModel> GetSelectCheckListAsync(long CodCompl)
        {
            try
            {
                using DatabaseContext db = new();
                var data = await db.CheckListGerals
                    .Where(c => c.codcompl == CodCompl)
                    .FirstOrDefaultAsync();
                CheckListGeral = data;
                return CheckListGeral;
            }
            catch (NpgsqlException)
            {
                throw;
            }
        }

        public async Task<ObservableCollection<QryCheckListGeralComplementoModel>> GetCheckListGeralComplementoAsync()
        {
            try
            {
                CheckListGeralComplementos = new ObservableCollection<QryCheckListGeralComplementoModel>();
                using DatabaseContext db = new();
                var data = await db.CheckListGeralComplementos
                    .OrderBy(c => c.coddetalhescompl)
                    .Where(c => c.codcompl == CheckListGeral.codcompl)
                    .ToListAsync();

                CheckListGeralComplementos = new ObservableCollection<QryCheckListGeralComplementoModel>(data);
                //return CheckListGeralComplementos;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //return new ObservableCollection<QryCheckListGeralComplementoModel>();
            return CheckListGeralComplementos;
        }

        public async Task<DetalhesComplemento> AddDetalhesComplementoCheckListAsync()
        {
            if (DetCompl == null)
            {
                throw new ArgumentNullException($"{nameof(AddDetalhesComplementoCheckListAsync)} entity must not be null");
            }

            try
            {
                using DatabaseContext db = new();
                db.Entry(DetCompl).State = DetCompl.coddetalhescompl == null ?
                                   EntityState.Added :
                                   EntityState.Modified;

                await db.SaveChangesAsync();

                return DetCompl;
            }
            catch (NpgsqlException)
            {
                throw;
            }
        }

        public async Task GetChkGeralRelatorioAsync()
        {
            try
            {
                //ChkGeralRelatorios = new ObservableCollection<ChkGeralRelatorioModel>();
                using DatabaseContext db = new();
                var data = await db.ChkGeralRelatorios
                    .OrderBy(c => c.ordem)
                    .Where(c => c.id_aprovado == Sigla.id_aprovado)
                    .Select(s => new
                    {
                        s.item_memorial,
                        s.local_shoppings,
                        s.planilha,
                        s.descricao_dd,
                        s.unidade,
                        s.qtd, //qtd = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:D}", s.qtd),
                        s.orient_montagem,
                        s.coddetalhescompl
                    })
                    .ToListAsync();
                ChkGeralRelatorios = data;
                //ChkGeralRelatorios = new ObservableCollection<ChkGeralRelatorioModel>(data);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propName)
        {
            //Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            //{
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            //}));
        }
        /*
        protected virtual void OnPropertyChanged(string propertyName)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
            }));

        }
        */

    }
}
