using HT.DataBase.Model;
using Syncfusion.UI.Xaml.Grid;
using System;
using System.Linq;

namespace HT.Views.Custom
{
    public class FuncionariosMultiColumnControl : SfMultiColumnDropDownControl //GridMultiColumnDropDownList //SfMultiColumnDropDownControl
    {

        
        protected override bool FilterRecord(object item)
        {
            var _item = item as FuncionarioAtivoModel;
            var result = (_item.nome_apelido.Contains(this.SearchText)) || (_item.codfun.Equals(this.SearchText));
            return result;
        }
        
    }
}
