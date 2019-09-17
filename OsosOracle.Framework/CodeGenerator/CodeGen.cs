using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace OsosOracle.Framework.CodeGenerator
{
    public class CodeGen
    {
        public string ProjeAdi = "";

        public string ProjeYolu => HttpContext.Current.Server.MapPath("~/App_Data/");

        //  public string[] SistemKolonlari = { "Id", "RowVersion", "EklemeTarihi", "EkleyenId", "DegistirenId", "DegistirmeTarihi", "Silindi" };
        public string[] SistemKolonlari = { "KAYITNO", "VERSIYON", "OLUSTURMATARIH", "OLUSTURAN", "GUNCELLEYEN", "GUNCELLEMETARIH", "Silindi" };

        private readonly string _dbserverIp;
        private readonly string _databaseAdi;
        private readonly string _sema;

        private readonly string _kullaniciAdi;
        private readonly string _sifre;


        public string EntitiesPath = @"Entities";
        public string DataLayerPath = @"DataLayer";
        public string BusinessPath = @"Business";
        public string WebUiPath = @"MvcUI";
        public string ServicePath = @"ServiceLayer";
        private List<string> _tableList;


        private bool _silindiKolonuVarmi = false;

        private StringBuilder _autocompleteFunctions = new StringBuilder();

        public CodeGen(string dbserverIp, string kullaniciAdi, string sifre, string databaseAdi, string sema = "dbo")
        {

            _dbserverIp = dbserverIp;
            _databaseAdi = databaseAdi;
            _kullaniciAdi = kullaniciAdi;
            _sifre = sifre;
            _sema = sema;

        }

        public void KodOlustur(string projeAdi, List<string> tableList)
        {


            ProjeAdi = projeAdi;
            if (ProjeAdi.Contains("*"))
            {
                throw new FormatException("Geçerli bir namespace belirtiniz");
            }



            if (tableList.Count < 1)
            {
                tableList = GetTableList().Select(t => t.Name).ToList();
            }

            _tableList = tableList;
            if (!Directory.Exists(ProjeYolu))
                Directory.CreateDirectory(ProjeYolu);


            foreach (var secilenTablo in tableList)
            {

                //  DataSet dsKolon = SqlSorguCalistir($@"select c.COLUMN_NAME,c.IS_NULLABLE,c.DATA_TYPE,c.CHARACTER_MAXIMUM_LENGTH
                //from INFORMATION_SCHEMA.COLUMNS c 
                //where c.table_name = '{secilenTablo}'  AND TABLE_SCHEMA ='{_sema}' ORDER BY c.ORDINAL_POSITION");

                DataSet dsKolon = SqlSorguCalistir($@"SELECT COLUMN_NAME as COLUMN_NAME ,NULLABLE as IS_NULLABLE,DATA_TYPE as DATA_TYPE,DATA_LENGTH as CHARACTER_MAXIMUM_LENGTH FROM user_tab_cols  WHERE table_name = '{secilenTablo}'");



                var kolonlar = (from DataRow row in dsKolon.Tables[0].Rows

                                select new KolonBilgi
                                {
                                    KolonAdi = row["COLUMN_NAME"].ToString(),
                                    Nullable = row["IS_NULLABLE"].ToString(),
                                    DataTipi = row["DATA_TYPE"].ToString(),
                                    MaxLength = row["CHARACTER_MAXIMUM_LENGTH"].ToString()
                                }).ToList();


                _silindiKolonuVarmi = SilindiKolonuVarmi(kolonlar);

                #region entities

                EntityOlustur(kolonlar, secilenTablo);
                AraComplexOlustur(kolonlar, secilenTablo);
                DetayComplexOlustur(secilenTablo, kolonlar);
                DataTableOlustur(secilenTablo);
                #endregion

                #region data layer

                EfEntityOlustur(secilenTablo);
                MappingOlustur(kolonlar, secilenTablo);
                DalOlustur(kolonlar, secilenTablo);
                IDalOlustur(secilenTablo);

                #endregion

                #region bussiness
                ValidationOlustur(kolonlar, secilenTablo);
                IBusinessOlustur(secilenTablo);
                BusinessOlustur(secilenTablo);
                #endregion


                #region wcf services

                //ServiceOlustur(secilenTablo);

                #endregion


                #region UI

                #region models
                KaydetModelOlustur(secilenTablo);
                IndexModelOlustur(secilenTablo);
                DetayModelOlustur(secilenTablo);
                #endregion

                #region Views
                IndexViewOlustur(kolonlar, secilenTablo);
                KaydetViewOlustur(kolonlar, secilenTablo);
                DetayViewOlustur(kolonlar, secilenTablo);
                #endregion


                ControllerOlustur(kolonlar, secilenTablo);
                #endregion

            }
            ReadMeOlustur(tableList);

            //ziple

            var folder = HttpContext.Current.Server.MapPath($"~/App_Data/") + " /" + ProjeAdi;
            FileStream fsOut = File.Create(folder + ".zip");
            ZipOutputStream zipStream = new ZipOutputStream(fsOut);

            zipStream.SetLevel(3); //0-9, 9 being the highest level of compression

            int folderOffset = folder.Length + (folder.EndsWith("\\") ? 0 : 1);

            CompressFolder(folder, zipStream, folderOffset);

            zipStream.IsStreamOwner = true; // Makes the Close also Close the underlying stream
            zipStream.Close();

            var dir = new DirectoryInfo(folder); //dosyaları sil
            dir.Delete(true);
        }


        #region readme.txt

        private void ReadMeOlustur(List<string> tabloListe)
        {

            StringBuilder sbServiceModule = new StringBuilder();
            StringBuilder sbAppcontextVars = new StringBuilder();
            StringBuilder sbAppcontextMaps = new StringBuilder();
            StringBuilder sbBussinesModuleService = new StringBuilder();
            StringBuilder sbBussinesModuleDal = new StringBuilder();
            StringBuilder sbValidators = new StringBuilder();

            StringBuilder sbAutoCompleteTables1 = new StringBuilder();
            StringBuilder sbAutoCompleteTables2 = new StringBuilder();
            StringBuilder sbAutoCompleteTables3 = new StringBuilder();

            int i = 1000;
            foreach (var tabloAdi in tabloListe)
            {
                //                string tabloAdiIlkHarfKucuk = IlkHarfiKucukYap(tabloAdi);


                sbServiceModule.AppendLine($"Bind<I{tabloAdi}Service>().ToConstant(WcfProxy<I{tabloAdi}Service>.CreateChannel()).InSingletonScope();");
                sbAppcontextVars.AppendLine($"public DbSet<{tabloAdi}Ef> {tabloAdi}Ef {{ get; set; }}");
                sbAppcontextMaps.AppendLine($"modelBuilder.Configurations.Add(new {tabloAdi}Map());");
                sbBussinesModuleService.AppendLine($"Bind<I{tabloAdi}Service>().To<{tabloAdi}Manager>().InSingletonScope();");
                sbBussinesModuleDal.AppendLine($"Bind<I{tabloAdi}Dal>().To<Ef{tabloAdi}Dal>().InSingletonScope();");



                sbValidators.AppendLine($"Bind<IValidator<{tabloAdi}>>().To<{tabloAdi}Validator>().InSingletonScope();");



                sbAutoCompleteTables1.AppendLine($@" [Description(""/{tabloAdi}/AjaxAra"")]");
                sbAutoCompleteTables1.AppendLine($@" {tabloAdi}Getir = {i},");

                sbAutoCompleteTables2.AppendLine($@" [Description(""/{tabloAdi}/AjaxTekDeger"")]");
                sbAutoCompleteTables2.AppendLine($@" {tabloAdi}Getir = {i},");

                sbAutoCompleteTables3.AppendLine($@" [Description(""/{tabloAdi}/AjaxCokDeger"")]");
                sbAutoCompleteTables3.AppendLine($@" {tabloAdi}Getir = {i},");
                i++;
            }

            var sb = new StringBuilder();

            sb.Append($@"
{ProjeAdi} Projesi Üretilmiş Kod



");

            sb.AppendLine("//AutoComplete functions");
            sb.Append(_autocompleteFunctions);

            sb.Append(sbAutoCompleteTables1);
            sb.AppendLine("");
            sb.Append(sbAutoCompleteTables2);
            sb.AppendLine("");
            sb.Append(sbAutoCompleteTables3);



            sb.AppendLine("");
            sb.AppendLine("//Service Modül Bindings");
            sb.Append(sbServiceModule);

            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("//Appcontext Properties");
            sb.Append(sbAppcontextVars);

            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("//Appcontext Mappings");
            sb.Append(sbAppcontextMaps);

            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("//BussinesModule Service Bindindgs");
            sb.Append(sbBussinesModuleService);

            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("//BussinesModule Data Access Layer bindings");
            sb.Append(sbBussinesModuleDal);

            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("//validator bindings");
            sb.Append(sbValidators);



            DosyaYaz(sb.ToString(), "", "ReadMe.txt");
        }


        #endregion


        #region Business

        private void ServiceOlustur(string tabloAdi)
        {
            string tabloAdiIlkHarfKucuk = IlkHarfiKucukYap(tabloAdi);

            StringBuilder sb = new StringBuilder();

            sb.Append($@"using System.Collections.Generic;
using {ProjeAdi}.Business.Abstract;
using {ProjeAdi}.Business.DependencyResolvers.Ninject;
using {ProjeAdi}.Entities.ComplexType.{tabloAdi}ComplexTypes;
using {ProjeAdi}.Entities.Concrete;

namespace {ProjeAdi}.ServiceLayer.Concrete
{{
	public class {tabloAdi}Service : TemelIsKurali, I{tabloAdi}Service
	{{
		private readonly I{tabloAdi}Service _{tabloAdiIlkHarfKucuk}Service;
		public {tabloAdi}Service()
		{{
			_{tabloAdiIlkHarfKucuk}Service = InstanceFactory<I{tabloAdi}Service>.GetInstance();
		}}

		public {tabloAdi} GetirById(int id)
		{{
			return _{tabloAdiIlkHarfKucuk}Service.GetirById(id);
		}}

		public {tabloAdi}Detay DetayGetirById(int id)
		{{
			return _{tabloAdiIlkHarfKucuk}Service.DetayGetirById(id);
		}}


		public List<{tabloAdi}> Getir({tabloAdi}Ara filtre)
		{{
			return _{tabloAdiIlkHarfKucuk}Service.Getir(filtre);
		}}
		public List<{tabloAdi}Detay> DetayGetir({tabloAdi}Ara filtre)
		{{
			return _{tabloAdiIlkHarfKucuk}Service.DetayGetir(filtre);
		}}

		public int KayitSayisiGetir({tabloAdi}Ara filtre)
		{{
			return _{tabloAdiIlkHarfKucuk}Service.KayitSayisiGetir(filtre);
		}}

		public {tabloAdi}DataTable Ara({tabloAdi}Ara filtre = null)
		{{
			return _{tabloAdiIlkHarfKucuk}Service.Ara(filtre);
		}}
		public {tabloAdi} Ekle({tabloAdi} {tabloAdiIlkHarfKucuk})
		{{
			return _{tabloAdiIlkHarfKucuk}Service.Ekle({tabloAdiIlkHarfKucuk});
		}}
		public {tabloAdi} Guncelle({tabloAdi} {tabloAdiIlkHarfKucuk})
		{{
			return _{tabloAdiIlkHarfKucuk}Service.Guncelle({tabloAdiIlkHarfKucuk});
		}}
		public void Sil(int id)
		{{
			_{tabloAdiIlkHarfKucuk}Service.Sil(id);
		}}
	}}
}}
");

            DosyaYaz(sb.ToString(), ServicePath + @"\Concrete", tabloAdi + "Service.cs");
        }

        #endregion

        #region Web

        private void KaydetModelOlustur(string tabloAdi)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($@"
using {ProjeAdi}.Entities.Concrete;
namespace {ProjeAdi}.MvcUI.Models.{tabloAdi}Models
{{
	public class {tabloAdi}KaydetModel
	{{
		public {tabloAdi} {tabloAdi} {{ get; set; }}
	}}
}}
");

            DosyaYaz(sb.ToString(), WebUiPath + @"\Models\" + tabloAdi + "Models", tabloAdi + "KaydetModel.cs");
        }


        private void IndexModelOlustur(string tabloAdi)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($@"
using {ProjeAdi}.Entities.ComplexType.{tabloAdi}ComplexTypes;
namespace {ProjeAdi}.MvcUI.Models.{tabloAdi}Models
{{
	public class {tabloAdi}IndexModel
	{{
		public {tabloAdi}Ara {tabloAdi}Ara {{ get; set; }}
	}}
}}
");

            DosyaYaz(sb.ToString(), WebUiPath + @"\Models\" + tabloAdi + "Models", tabloAdi + "IndexModel.cs");
        }

        private void DetayModelOlustur(string tabloAdi)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($@"
using {ProjeAdi}.Entities.ComplexType.{tabloAdi}ComplexTypes;
namespace {ProjeAdi}.MvcUI.Models.{tabloAdi}Models
{{
	public class {tabloAdi}DetayModel
	{{
		public {tabloAdi}Detay {tabloAdi}Detay {{ get; set; }}
	}}
}}
");

            DosyaYaz(sb.ToString(), WebUiPath + @"\Models\" + tabloAdi + "Models", tabloAdi + "DetayModel.cs");
        }

        private void IndexViewOlustur(List<KolonBilgi> kolonList, string tabloAdi)
        {


            var sb = new StringBuilder();
            var sbDataTableColumns = new StringBuilder();
            var sbDataTableHeaders = new StringBuilder();


            foreach (KolonBilgi kolon in kolonList)
            {
                if (!SistemKolonlari.Contains(kolon.KolonAdi)) //sadece gerekli kolonlar
                {
                    sb.AppendLine(SqlToIndexView(GetEnumValue<SqlDbType>(kolon.DataTipi),
                        kolon.KolonAdi, tableName: tabloAdi, onEk: tabloAdi + "Ara", enableValidation: false));

                    sbDataTableHeaders.AppendLine($@"			<th>@_T(""{SplitCamelCase(kolon.KolonAdi.Replace("Id", ""))}"")</th>");
                    sbDataTableColumns.AppendLine($@"				{{'data': ""{kolon.KolonAdi}""}},");
                }

            }

            var view = $@"
@using OsosOracle.Framework.HtmlHelpers
@using {ProjeAdi}.MvcUI.Infrastructure
@model {ProjeAdi}.MvcUI.Models.{tabloAdi}Models.{tabloAdi}IndexModel

@section panelHeading{{
	@Html.ButtonLink(_T(""Yeni {tabloAdi}"").ToString(), ""Ekle"", ""btn btn-xs btn-success modalizer"", ""fa-plus"")
}}

@using (Html.BeginPortlet(""{tabloAdi} Arama"", ""Detaylı Arama"", ""fa fa-search""))
{{
<form id=""frmAra"" class=""form-bordered"">

	{sb}

	<div class=""form-actions"">
		<button type=""button"" class=""btn btn-primary btn-sm"" id=""btnAra""><i class=""fa fa-search""></i> @_T(""Bul"")</button>
	</div>
</form>
}}


<table id=""tbl{tabloAdi}"" class=""table table-striped table-bordered table-hover table-responsive"">
	<thead>
		<tr>
			<th style=""width: 30px"">@_T(""Id"")</th>
			{sbDataTableHeaders}
			<th style=""width: 100px"">@_T(""İşlemler"")</th>
		</tr>
	</thead>
</table>
@section script{{
<script type=""text/javascript"">
	$(document).ready(function () {{
		AraFiltered();
		$(""#btnAra"").click(function(e) {{
				e.preventDefault();
				AraFiltered();
		}});
	}});

	function AraFiltered() {{
		$('#tbl{tabloAdi}').dataTable({{
			'language': dataTableLanguage,
			'processing': true,
			'serverSide': true,
			'destroy': true,
			'columns': [
				{{'data': ""KAYITNO""}},
				{sbDataTableColumns}
				{{'data': ""Islemler"", sortable: false}}
			],
			'ajax': {{
				'type': 'POST',
				'data': $(""#frmAra"").serializeObject(),
				'url': ""@Url.Action(""DataTablesList"", ""{tabloAdi}"")"",
				'error': function(xhr, ajaxOptions, errorThrown) {{ ajaxMesajGoster(xhr.responseJSON.Mesaj); }},
				'complete': function() {{ }}
		   }}
			
		}});
	 }}
   
</script>
}}";




            DosyaYaz(view, WebUiPath + @"\Views\" + tabloAdi, "Index.cshtml");
        }


        private void KaydetViewOlustur(List<KolonBilgi> kolonList, string tabloAdi)
        {

            var sb = new StringBuilder();

            foreach (KolonBilgi kolon in kolonList)
            {
                if (!SistemKolonlari.Contains(kolon.KolonAdi)) //sadece gerekli kolonlar
                {
                    sb.AppendLine(SqlToKaydetView(GetEnumValue<SqlDbType>(kolon.DataTipi),
                       kolon.KolonAdi, onEk: tabloAdi, tableName: tabloAdi));
                }

            }


            var view = $@"
@using {ProjeAdi}.MvcUI.Infrastructure
@using OsosOracle.Framework.HtmlHelpers
@model {ProjeAdi}.MvcUI.Models.{tabloAdi}Models.{tabloAdi}KaydetModel

@section panelHeading
{{
	
}}

<div class=""ajaxForm"">

	@using (Html.BeginForm(""Kaydet"", ""{tabloAdi}"", FormMethod.Post))
	{{
		@Html.AntiForgeryToken()
		@Html.HiddenFor(t => t.{tabloAdi}.KAYITNO)
		@Html.HiddenFor(t => t.{tabloAdi}.VERSIYON)
		@Html.ValidationSummary(true)

	  {sb}
	 
		<div class=""form-actions"">
			<button type=""submit"" class=""btn btn-primary""><i class=""fa fa-save""></i> @_T(""Kaydet"")</button>
			@Html.ButtonLink(_T(""Vazgeç"").ToString(), ""Index"", ""btn btn-xs btn-danger cancel"", ""fa-remove"")
		</div>
	}}
</div>

@section script
{{
	<script type=""text/javascript"">
		$(document).ready(function () {{
		//js kodunuzu buraya yazın	
		}});

	</script>
}}
";



            DosyaYaz(view, WebUiPath + @"\Views\" + tabloAdi, "Kaydet.cshtml");
        }


        private void DetayViewOlustur(List<KolonBilgi> kolonList, string tabloAdi)
        {


            var sb = new StringBuilder();

            foreach (KolonBilgi kolon in kolonList)
            {
                if (!SistemKolonlari.Contains(kolon.KolonAdi)) //sadece gerekli kolonlar
                {
                    sb.AppendLine(SqlToDisplay(GetEnumValue<SqlDbType>(kolon.DataTipi),
                        kolon.KolonAdi, onEk: tabloAdi + "Detay"));
                }

            }


            var view = $@"

@using OsosOracle.Framework.HtmlHelpers
@model {ProjeAdi}.MvcUI.Models.{tabloAdi}Models.{tabloAdi}DetayModel

@section panelHeading{{
	@Html.ButtonLink(_T(""Listeye Dön"").ToString(), ""Index"", ""btn btn-xs btn-info"", ""fa-reply"")
	@Html.ButtonLink(_T(""Sil"").ToString(), ""Sil"", ""btn btn-xs btn-danger modalizer"", ""fa-trash"", new {{ id = Model.{tabloAdi}Detay.Id }})
	@Html.ButtonLink(_T(""Düzenle"").ToString(), ""Guncelle"",""btn btn-xs btn-primary modalizer"", ""fa-edit"", new {{ id = Model.{tabloAdi}Detay.Id }})

}}

<div class=""ajaxForm"">

	  {sb}
	 
	@Html.ButtonLink(_T(""Vazgeç"").ToString(), ""Index"", ""btn btn-xs btn-danger cancel"", ""fa-remove"")
</div>

@section script
{{
	<script type=""text/javascript"">
		$(document).ready(function () {{
		//js kodunuzu buraya yazın	
		}});

	</script>
}}
";



            DosyaYaz(view, WebUiPath + @"\Views\" + tabloAdi, "Detay.cshtml");
        }



        private void ControllerOlustur(List<KolonBilgi> kolonList, string tabloAdi)
        {
            string tabloAdiIlkHarfKucuk = IlkHarfiKucukYap(tabloAdi);

            StringBuilder sb = new StringBuilder();
            StringBuilder sbDataTableColumns = new StringBuilder();


            foreach (KolonBilgi kolon in kolonList)
            {
                if (!SistemKolonlari.Contains(kolon.KolonAdi)) //sadece gerekli kolonlar
                {
                    sbDataTableColumns.AppendLine($@"				t.{kolon.KolonAdi},");
                }

            }


            sb.Append($@"

using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using {ProjeAdi}.Business.Abstract;
using {ProjeAdi}.Entities.ComplexType.{tabloAdi}ComplexTypes;
using {ProjeAdi}.Entities.Concrete;
using {ProjeAdi}.Entities.Enums;
using {ProjeAdi}.MvcUI.Infrastructure;
using {ProjeAdi}.MvcUI.Models.{tabloAdi}Models;
using OsosOracle.Framework.Web.Mvc;
using OsosOracle.Framework.SharedModels;
using OsosOracle.Framework.Utilities.ExtensionMethods;
using OsosOracle.Framework.Enums;
using OsosOracle.Framework.DataAccess.Filter;

namespace {ProjeAdi}.MvcUI.Controllers
{{
    [AuthorizeUser]
	public class {tabloAdi}Controller : BaseController
	{{
		private readonly I{tabloAdi}Service _{tabloAdiIlkHarfKucuk}Service;

		public {tabloAdi}Controller(I{tabloAdi}Service {tabloAdiIlkHarfKucuk}Service)
		{{
			_{tabloAdiIlkHarfKucuk}Service = {tabloAdiIlkHarfKucuk}Service;
		}}

		
		public ActionResult Index()
		{{
			SayfaBaslik($""{tabloAdi} İşlemleri"");
			var model= new {tabloAdi}IndexModel();
			return View(model);
		}}

		[HttpPost]
		public ActionResult DataTablesList(DtParameterModel dtParameterModel, {tabloAdi}Ara {tabloAdiIlkHarfKucuk}Ara)
		{{
			
			{tabloAdiIlkHarfKucuk}Ara.Ara = dtParameterModel.AramaKriteri;
			
			if (!string.IsNullOrEmpty(dtParameterModel.Search.Value))
			{{ //TODO: Bu bölümü düzenle
				{tabloAdiIlkHarfKucuk}Ara.Adi = dtParameterModel.Search.Value;
			}}

	 

			var kayitlar = _{tabloAdiIlkHarfKucuk}Service.Ara({tabloAdiIlkHarfKucuk}Ara);

			return Json(new DataTableResult()
			{{
				data = kayitlar.{tabloAdi}DetayList.Select(t => new
				{{
					//TODO: Bu bölümü düzenle
					t.KAYITNO,
{sbDataTableColumns}
					Islemler =$@""<a class='btn btn-xs btn-info modalizer' href='{{Url.Action(""Guncelle"", ""{tabloAdi}"", new {{id = t.KAYITNO}})}}' title='Düzenle'><i class='fa fa-edit'></i></a>
							   <a class='btn btn-xs btn-primary' href='{{Url.Action(""Detay"", ""{tabloAdi}"", new {{id = t.KAYITNO}})}}' title='Detay'><i class='fa fa-th-list'></i></a>
								<a class='btn btn-xs btn-danger modalizer' href='{{Url.Action(""Sil"", ""{tabloAdi}"", new {{id = t.KAYITNO}})}}' title='Sil'><i class='fa fa-trash'></i></a>""
				}}),
				draw = dtParameterModel.Draw,
				recordsTotal = kayitlar.ToplamKayitSayisi,
				recordsFiltered = kayitlar.ToplamKayitSayisi
			}}, JsonRequestBehavior.AllowGet);
		}}

		
		public ActionResult Ekle()
		{{
			SayfaBaslik($""{tabloAdi} Ekle"");

			var model = new {tabloAdi}KaydetModel
			{{
				{tabloAdi} = new {tabloAdi}()
			}};
			
	

			return View(""Kaydet"", model);
		}}

		
		public ActionResult Guncelle(int id)
		{{
			SayfaBaslik($""{tabloAdi} Güncelle"");

			var model = new {tabloAdi}KaydetModel
			{{
				{tabloAdi} = _{tabloAdiIlkHarfKucuk}Service.GetirById(id)
			}};

	
			return View(""Kaydet"", model);
		}}


		
		public ActionResult Detay(int id)
		{{
			SayfaBaslik($""{tabloAdi} Detay"");

			var model = new {tabloAdi}DetayModel
			{{
				{tabloAdi}Detay = _{tabloAdiIlkHarfKucuk}Service.DetayGetirById(id)
			}};

	
			return View(""Detay"", model);
		}}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Kaydet({tabloAdi}KaydetModel {tabloAdiIlkHarfKucuk}KaydetModel)
		{{
			if ({tabloAdiIlkHarfKucuk}KaydetModel.{tabloAdi}.KAYITNO > 0)
			{{
				_{tabloAdiIlkHarfKucuk}Service.Guncelle({tabloAdiIlkHarfKucuk}KaydetModel.{tabloAdi}.List());
			}}
			else
			{{
				_{tabloAdiIlkHarfKucuk}Service.Ekle({tabloAdiIlkHarfKucuk}KaydetModel.{tabloAdi}.List());
			}}

			return Yonlendir(Url.Action(""Index""), $""{tabloAdi} kayıdı başarıyla gerçekleştirilmiştir."");
			//return Yonlendir(Url.Action(""Detay"",""{tabloAdi}"",new{{id={tabloAdiIlkHarfKucuk}KaydetModel.{tabloAdi}.Id}}), $""{tabloAdi} kayıdı başarıyla gerçekleştirilmiştir."");
		}}


		
		public ActionResult Sil(int id)
		{{
			SayfaBaslik($""{tabloAdi} Silme İşlem Onayı"");
			return View(""_SilOnay"", new DeleteViewModel() {{ Id = id, RedirectUrlForCancel =  $""/{tabloAdi}/Index/{{id}}"" }});
		}}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Sil(DeleteViewModel model)
		{{
			_{tabloAdiIlkHarfKucuk}Service.Sil(model.Id.List());

			return Yonlendir(Url.Action(""Index""), $""{tabloAdi} Başarıyla silindi"");
		}}


		public ActionResult AjaxAra(string key, {tabloAdi}Ara {tabloAdiIlkHarfKucuk}Ara=null, int limit = 10, int baslangic = 0)
		{{
			
			 if ({tabloAdiIlkHarfKucuk}Ara == null)
			{{
				{tabloAdiIlkHarfKucuk}Ara = new {tabloAdi}Ara();
			}}

            {tabloAdiIlkHarfKucuk}Ara.Ara = new Ara
            {{
                Baslangic = baslangic,
                Uzunluk = limit,
                Siralama = new List<Siralama>
                {{
                    new Siralama
                    {{
                        KolonAdi = LinqExtensions.GetPropertyName(({tabloAdi} t) => t.KAYITNO),
                        SiralamaTipi = EnumSiralamaTuru.Asc
                    }}
                }}
            }};


			//TODO: Bu bölümü düzenle
			{tabloAdiIlkHarfKucuk}Ara.Adi = key;

			var {tabloAdiIlkHarfKucuk}List =  _{tabloAdiIlkHarfKucuk}Service.Getir({tabloAdiIlkHarfKucuk}Ara);


			var data = {tabloAdiIlkHarfKucuk}List.Select({tabloAdiIlkHarfKucuk} => new AutoCompleteData
			{{
			//TODO: Bu bölümü düzenle
				id = {tabloAdiIlkHarfKucuk}.Id.ToString(),
				text = {tabloAdiIlkHarfKucuk}.Id.ToString(),
				description = {tabloAdiIlkHarfKucuk}.Id.ToString(),
			}}).ToList();
			return Json(data, JsonRequestBehavior.AllowGet);
		}}


		public ActionResult AjaxTekDeger(int id)
		{{
			var {tabloAdiIlkHarfKucuk} = _{tabloAdiIlkHarfKucuk}Service.GetirById(id);


			var data = new AutoCompleteData
			{{//TODO: Bu bölümü düzenle
				id = {tabloAdiIlkHarfKucuk}.Id.ToString(),
				text = {tabloAdiIlkHarfKucuk}.Id.ToString(),
				description = {tabloAdiIlkHarfKucuk}.Id.ToString(),
			}};

			return Json(data, JsonRequestBehavior.AllowGet);
		}}

		public ActionResult AjaxCokDeger(string id)
		{{
			var {tabloAdiIlkHarfKucuk}List = _{tabloAdiIlkHarfKucuk}Service.Getir(new {tabloAdi}Ara() {{ KAYITNOlar = id }});


			var data = {tabloAdiIlkHarfKucuk}List.Select({tabloAdiIlkHarfKucuk} => new AutoCompleteData
			{{ //TODO: Bu bölümü düzenle
				id = {tabloAdiIlkHarfKucuk}.Id.ToString(),
				text = {tabloAdiIlkHarfKucuk}.Id.ToString(),
				description = {tabloAdiIlkHarfKucuk}.Id.ToString()
			}});

			return Json(data, JsonRequestBehavior.AllowGet);
		}}

	}}
}}

");


            DosyaYaz(sb.ToString(), WebUiPath + @"\Controllers", tabloAdi + "Controller.cs");




        }




        #endregion

        #region Business

        private void IBusinessOlustur(string tabloAdi)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($@"
using System.Collections.Generic;
using System.ServiceModel;
using {ProjeAdi}.Entities.ComplexType.{tabloAdi}ComplexTypes;
using {ProjeAdi}.Entities.Concrete;

namespace {ProjeAdi}.Business.Abstract
{{
	[ServiceContract]
	public interface I{tabloAdi}Service
	{{
		[OperationContract]
		{tabloAdi} GetirById(int id);

		[OperationContract]
		{tabloAdi}Detay DetayGetirById(int id);

		[OperationContract]
		List<{tabloAdi}> Getir({tabloAdi}Ara filtre = null);

		[OperationContract]
		List<{tabloAdi}Detay> DetayGetir({tabloAdi}Ara filtre = null);

		[OperationContract]
		int KayitSayisiGetir({tabloAdi}Ara filtre = null);


		[OperationContract]
		{tabloAdi}DataTable Ara({tabloAdi}Ara filtre = null);

		[OperationContract]
		void Ekle(List<{tabloAdi}> entityler);

		[OperationContract]
		void Guncelle(List<{tabloAdi}> entityler);

		[OperationContract]
		void Sil(List<int> idler);
	}}
}}
");

            DosyaYaz(sb.ToString(), BusinessPath + @"\Abstract", "I" + tabloAdi + "Service.cs");
        }

        private void BusinessOlustur(string tabloAdi)
        {
            string tabloAdiIlkHarfKucuk = IlkHarfiKucukYap(tabloAdi);

            StringBuilder sb = new StringBuilder();

            sb.Append($@"using System.Collections.Generic;
using {ProjeAdi}.Business.Abstract;
using {ProjeAdi}.Business.ValidationRules.FluentValidation;
using {ProjeAdi}.DataLayer.Abstract;
using {ProjeAdi}.DataLayer.Concrete.EntityFramework.Entity;
using {ProjeAdi}.Entities.ComplexType.{tabloAdi}ComplexTypes;
using {ProjeAdi}.Entities.Concrete;
using {ProjeAdi}.Business.Concrete.Infrastructure;

namespace {ProjeAdi}.Business.Concrete
{{
	public class {tabloAdi}Manager : BaseManager, I{tabloAdi}Service
	{{
		private readonly I{tabloAdi}Dal _{tabloAdiIlkHarfKucuk}Dal;
		public {tabloAdi}Manager(I{tabloAdi}Dal {tabloAdiIlkHarfKucuk}Dal)
		{{
			_{tabloAdiIlkHarfKucuk}Dal = {tabloAdiIlkHarfKucuk}Dal;
		}}

		public {tabloAdi} GetirById(int id)
		{{
			return _{tabloAdiIlkHarfKucuk}Dal.Getir(id);
		}}

		public {tabloAdi}Detay DetayGetirById(int id)
		{{
			return _{tabloAdiIlkHarfKucuk}Dal.DetayGetir(id);
		}}

		public List<{tabloAdi}> Getir({tabloAdi}Ara filtre)
		{{
			return _{tabloAdiIlkHarfKucuk}Dal.Getir(filtre);
		}}

		public int KayitSayisiGetir({tabloAdi}Ara filtre)
		{{
			return _{tabloAdiIlkHarfKucuk}Dal.KayitSayisiGetir(filtre);
		}}

		public List<{tabloAdi}Detay> DetayGetir({tabloAdi}Ara filtre)
		{{
			return _{tabloAdiIlkHarfKucuk}Dal.DetayGetir(filtre);
		}}

		public {tabloAdi}DataTable Ara({tabloAdi}Ara filtre = null)
		{{
			return _{tabloAdiIlkHarfKucuk}Dal.Ara(filtre);
		}}

		
		[FluentValidationAspect(typeof({tabloAdi}Validator))]
		private void Validate({tabloAdi} entity)
		{{
			//Kontroller Yapılacak
		}}

		public void Ekle(List<{tabloAdi}> entityler)
		{{
			foreach (var entity in entityler)
			{{
				Validate(entity);
			}}
			_{tabloAdiIlkHarfKucuk}Dal.Ekle(entityler.ConvertEfList<{tabloAdi}, {tabloAdi}Ef>());
		}}

		public void Guncelle(List<{tabloAdi}> entityler)
		{{
			foreach (var entity in entityler)
			{{
				Validate(entity);
			}}
			_{tabloAdiIlkHarfKucuk}Dal.Guncelle(entityler.ConvertEfList<{tabloAdi}, {tabloAdi}Ef>());
		}}

		public void Sil(List<int> idler)
		{{
			_{tabloAdiIlkHarfKucuk}Dal.Sil(idler);
		}}

	}}
}}");

            DosyaYaz(sb.ToString(), BusinessPath + @"\Concrete", tabloAdi + "Manager.cs");
        }

        private void ValidationOlustur(List<KolonBilgi> kolonList, string tabloAdi)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("using FluentValidation;");
            sb.AppendLine("using " + ProjeAdi + ".Entities.Concrete;");
            sb.AppendLine("");
            sb.AppendLine("namespace " + ProjeAdi + ".Business.ValidationRules.FluentValidation");
            sb.AppendLine("{");
            sb.AppendLine("public class " + tabloAdi + "Validator : AbstractValidator<" + tabloAdi + ">");
            sb.AppendLine("{");
            sb.AppendLine("public " + tabloAdi + "Validator()");
            sb.AppendLine("{");

            foreach (KolonBilgi kolon in kolonList.FindAll(t => t.Nullable == "N"))
            {
                //sistem kolonlarında validasyon olamayacak
                if (!SistemKolonlari.Contains(kolon.KolonAdi)) //KayıtNo kolonu validasyondan geçmez
                {
                    sb.AppendLine(
                        $"RuleFor(t => t.{kolon.KolonAdi}).NotEmpty().WithMessage(\"{SplitCamelCase(kolon.KolonAdi.Replace("KAYITNO", ""))} giriniz\");");
                }
            }

            sb.AppendLine("");

            foreach (KolonBilgi kolon in kolonList)
            {
                if (kolon.DataTipi == "VARCHAR2")
                {
                    if (!string.IsNullOrEmpty(kolon.MaxLength))
                        if (Convert.ToInt32(kolon.MaxLength) >= 1)
                            sb.AppendLine($"RuleFor(t => t.{kolon.KolonAdi}).Length(1, {kolon.MaxLength}).WithMessage(\"{kolon.KolonAdi} {kolon.MaxLength} karakterden büyük olamaz\");");

                }

            }


            sb.AppendLine("}");
            sb.AppendLine("}");
            sb.AppendLine("}");

            DosyaYaz(sb.ToString(), BusinessPath + @"\ValidationRules\FluentValidation", tabloAdi + "Validator.cs");
        }

        #endregion

        #region Dal

        private void IDalOlustur(string tabloAdi)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($@"using System.Collections.Generic;
using {ProjeAdi}.DataLayer.Concrete.EntityFramework.Entity;
using {ProjeAdi}.Entities.ComplexType.{tabloAdi}ComplexTypes;
using {ProjeAdi}.Entities.Concrete;

namespace {ProjeAdi}.DataLayer.Abstract
{{
	public interface I{tabloAdi}Dal
	{{
		{tabloAdi} Getir(int id);
		{tabloAdi}Detay DetayGetir(int id);

		List<{tabloAdi}> Getir({tabloAdi}Ara filtre = null);
		List<{tabloAdi}Detay> DetayGetir({tabloAdi}Ara filtre = null);

		{tabloAdi}DataTable Ara({tabloAdi}Ara filtre = null);

		int KayitSayisiGetir({tabloAdi}Ara filtre = null);

		List<{tabloAdi}> Ekle(List<{tabloAdi}Ef> entityler);
		void Guncelle(List<{tabloAdi}Ef> yeniDegerler);
		void Sil(List<int> idler);
	}}
}}");

            DosyaYaz(sb.ToString(), DataLayerPath + @"\Abstract", "I" + tabloAdi + "Dal.cs");
        }

        private void DalOlustur(List<KolonBilgi> kolonList, string tabloAdi)
        {
            StringBuilder sb = new StringBuilder();

            var silindi = _silindiKolonuVarmi ? "result = result.Where(x => x.Silindi == false);" : "";


            sb.Append($@"using System.Collections.Generic;
using System.Linq;
using {ProjeAdi}.DataLayer.Abstract;
using {ProjeAdi}.DataLayer.Concrete.EntityFramework.Entity;
using {ProjeAdi}.Entities.ComplexType.{tabloAdi}ComplexTypes;
using {ProjeAdi}.Entities.Concrete;


namespace {ProjeAdi}.DataLayer.Concrete.EntityFramework.Dal
{{
	public class Ef{tabloAdi}Dal : I{tabloAdi}Dal
	{{


		private IQueryable<{tabloAdi}Detay> DetayDoldur(IQueryable<{tabloAdi}Ef> result)
		{{
			return result.Select(x => new {tabloAdi}Detay()
			{{
				//	{tabloAdi} = x,
				 KAYITNO = x.KAYITNO,

");

            foreach (KolonBilgi kolon in kolonList)
            {
                if (!SistemKolonlari.Contains(kolon.KolonAdi)) //sadece gerekli kolonlar
                {
                    sb.AppendLine($" {kolon.KolonAdi} = x.{kolon.KolonAdi},");
                }
            }



            sb.Append($@"			
				//TODO: Ek detayları buraya ekleyiniz
				//örnek: {tabloAdi}Durumu = x.NesneDegerDurumEf.Adi

			}});
		}}

		


		private IQueryable<{tabloAdi}Ef> Filtrele(IQueryable<{tabloAdi}Ef> result, {tabloAdi}Ara filtre = null)
		{{
			//silindi kolonu varsa silinenler gelmesin
			{silindi}
			//TODO: filtereyi özelleştir
			if (filtre != null)
			{{
				//id ve idler
				if (filtre.KAYITNO != null){{result = result.Where(x => x.KAYITNO == filtre.KAYITNO);}}
				else {{

				//if (!string.IsNullOrEmpty(filtre.Idler))
				//{{
				//	var idList = filtre.Idler.ToList<int>();
				//	result = result.Where(x => idList.Contains(x.KAYITNO));
				//}}
");


            //Tüm kolonları arama filtresi ekle
            foreach (KolonBilgi kolon in kolonList)
            {
                if (!SistemKolonlari.Contains(kolon.KolonAdi)) //sadece gerekli kolonlar
                {

                    string netType = OracleToCSharp(GetEnumValue<MyOracleType>(kolon.DataTipi));

                    //string check Contains ile arama
                    if (netType == "string")
                    {
                        sb.Append($@"
				   if (!string.IsNullOrEmpty(filtre.{kolon.KolonAdi}))
					{{
						result = result.Where(x => x.{kolon.KolonAdi}.Contains(filtre.{kolon.KolonAdi}));
					}}");

                    }
                    else if (netType == "byte[]")
                    {
                        //byte array ile arama yapma
                    }
                    else
                    {
                        sb.Append($@"
				   if (filtre.{kolon.KolonAdi} != null)
					{{
						result = result.Where(x => x.{kolon.KolonAdi} == filtre.{kolon.KolonAdi});
					}}");
                    }


                }

            }
            sb.Append($@"
			}}
			}}
			return result;
		 }}

		public List<{tabloAdi}> Getir({tabloAdi}Ara filtre = null)
		{{
			using (var context = new AppContext())
			{{
				return GetirWithContext(context, filtre);
			}}
		}}

		public List<{tabloAdi}> GetirWithContext(AppContext context, {tabloAdi}Ara filtre = null)
		{{
			var filterHelper = new FilterHelper<{tabloAdi}Ef>();
			return filterHelper.Sayfala(Filtrele(context.{tabloAdi}Ef.AsQueryable(), filtre), filtre?.Ara).ToList<{tabloAdi}>();
		}}


		public List< {tabloAdi}Detay> DetayGetir( {tabloAdi}Ara filtre = null)
		{{
			using (var context = new AppContext())
			{{
				var filterHelper = new FilterHelper<{tabloAdi}Detay>();
				return filterHelper.Sayfala(DetayDoldur(Filtrele(context.{tabloAdi}Ef.AsQueryable(), filtre)), filtre?.Ara).ToList();
				
			}}
		}}


		public {tabloAdi}DataTable Ara({tabloAdi}Ara filtre = null)
		{{
			using (var context = new AppContext())
			{{
				 var filterHelper = new FilterHelper<{tabloAdi}Detay>();
				
				return new {tabloAdi}DataTable
				{{
					{tabloAdi}DetayList = filterHelper.Sayfala(DetayDoldur(Filtrele(context.{tabloAdi}Ef.AsQueryable(), filtre)), filtre?.Ara).ToList(),ToplamKayitSayisi = filterHelper.KayitSayisi
				}};
			}}
		}}

		public int KayitSayisiGetir({tabloAdi}Ara filtre = null)
		{{
			using (var context = new AppContext())
			{{
				var filterHelper = new FilterHelper<{tabloAdi}Ef>();
				return filterHelper.KayitSayisiGetir(Filtrele(context.{tabloAdi}Ef.AsQueryable(), filtre), filtre?.Ara);
			}}
		 }}
   ");



            sb.Append($@"

		public {tabloAdi} Getir(int id)
		{{
			using (var context = new AppContext())
			{{
				return context.{tabloAdi}Ef.Find(id);
			}}
		}}

		public {tabloAdi}Detay DetayGetir(int id)
		{{
			using (var context = new AppContext())
			{{
				 return DetayDoldur(Filtrele(context.{tabloAdi}Ef.AsQueryable(), new {tabloAdi}Ara() {{ KAYITNO = id }}))
				   .FirstOrDefault();
			}}
		}}

		public List<{tabloAdi}> Ekle(List<{tabloAdi}Ef> entityler)
		{{
			using (var context = new AppContext())
			{{
				if (entityler.Count == 1)
				{{
					var eklenen = context.{tabloAdi}Ef.Add(entityler[0]);
					context.SaveChanges();
					return new List<{tabloAdi}> {{ eklenen }};
				}}
				if (entityler.Count > 1)
				{{
					var eklenen = context.{tabloAdi}Ef.AddRange(entityler);
					context.SaveChanges();
					return eklenen.ToList<{tabloAdi}>();
				}}

                return null;
			}}
		}}


		public void Guncelle(List<{tabloAdi}Ef> yeniDegerler)
		{{
			using (var context = new AppContext())
			{{
				var mevcutDegerler = new List<{tabloAdi}Ef>();
				if (yeniDegerler.Count == 0)
					throw new NotificationException(\""Değer yok.\"");
				if (yeniDegerler.Count == 1)
				{{
					mevcutDegerler.Add(context.{tabloAdi}Ef.Find(yeniDegerler[0].KAYITNO));
				}}
				else
				{{
					var idler = yeniDegerler.Select(y => y.KAYITNO).ToList();
					mevcutDegerler = context.{tabloAdi}Ef.Where(x => idler.Contains(x.KAYITNO)).ToList();
				}}

				AlanlariGuncelle(context, mevcutDegerler, yeniDegerler);
				context.SaveChanges();
			}}
		}}

		private static void AlanlariGuncelle(AppContext context, List<{tabloAdi}Ef> mevcutDegerler, List<{tabloAdi}Ef> yeniDegerler)
		{{
			foreach (var yeniDeger in yeniDegerler)
			{{
				var mevcutDeger = mevcutDegerler.Find(x => x.KAYITNO == yeniDeger.KAYITNO);
				if (mevcutDeger == null)
					throw new NotificationException(\""{tabloAdi} bulunamadı\"");

				var entry = context.Entry(mevcutDeger);
				entry.CurrentValues.SetValues(yeniDeger);

				////Değişmemesi gereken kolonlar buraya yazılacak.
				//entry.Property(u => u.Id).IsModified = false;
				//entry.Property(u => u.EklemeTarihi).IsModified = false;
				//entry.Property(u => u.EkleyenId).IsModified = false;
			}}
		}}

		public void Sil(List<int> idler)
		{{
			using (var context = new AppContext())
			{{
				if (idler.Count == 1)
				{{
					var entry = context.{tabloAdi}Ef.Find(idler[0]);
					if (entry != null)
						context.{tabloAdi}Ef.Remove(entry);
				}}
				else if (idler.Count > 1)
				{{
					var entry = context.{tabloAdi}Ef.Where(x => idler.Contains(x.KAYITNO)).ToList();
					context.{tabloAdi}Ef.RemoveRange(entry);
				}}

				context.SaveChanges();
			}}
		}}

	}}
}}
");


            DosyaYaz(sb.ToString(), DataLayerPath + @"\Concrete\EntityFramework\Dal", "Ef" + tabloAdi + "Dal.cs");

        }

        private void EfEntityOlustur(string tabloAdi)
        {
            StringBuilder sb = new StringBuilder();


            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine($"using {ProjeAdi}.Entities.Concrete;");
            sb.AppendLine("");
            sb.AppendLine($"namespace {ProjeAdi}.DataLayer.Concrete.EntityFramework.Entity");
            sb.AppendLine(" {");
            sb.AppendLine($"    public sealed class {tabloAdi}Ef : {tabloAdi}");
            sb.AppendLine("    {");



            //Alt tablolar 1->many connection
            //var dsIliskiliTablolar = SqlSorguCalistir(@"select t.name as TABLE_NAME,c.name as COLUMN_NAME
            //   from sys.foreign_key_columns as fk
            //		inner join sys.tables as t on fk.parent_object_id = t.object_id
            //		inner join sys.columns as c on fk.parent_object_id = c.object_id and fk.parent_column_id = c.column_id
            //   where fk.referenced_object_id = (select object_id from sys.tables where name = '" + tabloAdi + "' and SCHEMA_NAME(schema_id) = '" + _sema + "') ");
            var dsIliskiliTablolar = SqlSorguCalistir(@"SELECT a.table_name as TABLE_NAME, a.column_name as COLUMN_NAME 
                FROM all_cons_columns a
                JOIN all_constraints c ON a.owner = c.owner AND a.constraint_name = c.constraint_name
                JOIN all_constraints c_pk ON c.r_owner = c_pk.owner AND c.r_constraint_name = c_pk.constraint_name
                join USER_CONS_COLUMNS uc on uc.constraint_name = c.r_constraint_name
                WHERE   uc.table_name='" + tabloAdi + "'");
            if (dsIliskiliTablolar.Tables[0].Rows.Count > 0)
            {
                sb.AppendLine($"public {tabloAdi}Ef()"); //constructor
                sb.AppendLine("{");
                foreach (DataRow drKolon in dsIliskiliTablolar.Tables[0].Rows)
                {
                    var tableName = drKolon["TABLE_NAME"].ToString();
                    var kolonName = drKolon["COLUMN_NAME"].ToString().Replace("Id", "");


                    if (tableName == tabloAdi)
                        sb.AppendLine($"Alt{tableName}EfCollection = new List<{tableName}Ef>();");
                    else
                    {
                        sb.AppendLine($"{kolonName}{tableName}EfCollection = new List<{tableName}Ef>();");
                    }

                }
                sb.AppendLine("}");
            }

            sb.AppendLine("");
            foreach (DataRow drKolon in dsIliskiliTablolar.Tables[0].Rows)
            {
                var tableName = drKolon["TABLE_NAME"].ToString();
                var kolonName = drKolon["COLUMN_NAME"].ToString().Replace("Id", "");

                if (tableName == tabloAdi)
                {
                    sb.AppendLine($"public  ICollection<{tableName}Ef> Alt{tableName}EfCollection {{ get; set; }}");
                }
                else
                {

                    sb.AppendLine($"public  ICollection<{tableName}Ef> {kolonName}{tableName}EfCollection {{ get; set; }}");
                }
            }

            //     //üst tablolar
            //dsIliskiliTablolar = SqlSorguCalistir(@"select (select name from sys.tables where object_id=fk.referenced_object_id) as TABLE_NAME,c.name as COLUMN_NAME
            //from sys.foreign_key_columns as fk
            //	inner join sys.tables as t on fk.parent_object_id = t.object_id
            //	inner join sys.columns as c on fk.parent_object_id = c.object_id and fk.parent_column_id = c.column_id
            //where fk.parent_object_id = (select object_id from sys.tables where name = '" + tabloAdi + "' and SCHEMA_NAME(schema_id) = '" + _sema + "') ");

            dsIliskiliTablolar = SqlSorguCalistir(@" SELECT  c_pk.table_name as TABLE_NAME,  a.column_name as COLUMN_NAME
                                                     FROM all_cons_columns a
                                                     JOIN all_constraints c ON a.owner = c.owner AND a.constraint_name = c.constraint_name
                                                     JOIN all_constraints c_pk ON c.r_owner = c_pk.owner AND c.r_constraint_name = c_pk.constraint_name
                                                     WHERE c.constraint_type = 'R' AND a.table_name = '" + tabloAdi + "'");
            sb.AppendLine("");
            foreach (DataRow drKolon in dsIliskiliTablolar.Tables[0].Rows)
            {
                if (drKolon["TABLE_NAME"].ToString() == tabloAdi)
                    sb.AppendLine($"public  {drKolon["TABLE_NAME"]}Ef Ust{drKolon["TABLE_NAME"]}Ef {{ get; set; }}");
                else
                {
                    var tableName = drKolon["COLUMN_NAME"].ToString().Replace("Id", "");

                    if (tableName != drKolon["TABLE_NAME"].ToString())
                    {
                        tableName = $"{drKolon["TABLE_NAME"]}{drKolon["COLUMN_NAME"].ToString().Replace("Id", "")}";
                    }
                    sb.AppendLine($"public  {drKolon["TABLE_NAME"]}Ef {tableName}Ef {{ get; set; }}");
                }

            }

            sb.AppendLine("}");
            sb.AppendLine("}");
            DosyaYaz(sb.ToString(), DataLayerPath + @"\Concrete\EntityFramework\Entity", tabloAdi + "Ef.cs");

        }

        private void MappingOlustur(List<KolonBilgi> kolonList, string tabloAdi)
        {

            List<string> nullableColums = new List<string>();

            StringBuilder sb = new StringBuilder();

            sb.Append($@"using System.ComponentModel.DataAnnotations.Schema;
			using System.Data.Entity.ModelConfiguration;
			using {ProjeAdi}.DataLayer.Concrete.EntityFramework.Entity;
			
			namespace {ProjeAdi}.DataLayer.Concrete.EntityFramework.Mappings
			{{
				public class {tabloAdi}Map : EntityTypeConfiguration<{tabloAdi}Ef>
				{{
					public {tabloAdi}Map()
					{{
		
					// Table & Column Mappings
					ToTable(""{tabloAdi}"");
					// Primary Key
					HasKey(t => t.KAYITNO);
					// Properties
			");


            foreach (KolonBilgi kolon in kolonList)
            {

                if (kolon.Nullable == "Y") //null olabilecek kolonları bul
                    nullableColums.Add(kolon.KolonAdi);

                sb.Append($"Property(t => t.{kolon.KolonAdi}).HasColumnName(\"{kolon.KolonAdi}\"){(kolon.KolonAdi == "RowVersion" ? ".IsConcurrencyToken()" : "")}");


                if (!string.IsNullOrEmpty(kolon.MaxLength))
                {
                    if (kolon.Nullable == "N") sb.Append(".IsRequired()");

                    //if (kolon.MaxLength != "-1")
                    //{
                    //    sb.Append($".HasMaxLength({kolon.MaxLength})");
                    //}


                }

                sb.Append(";");
                sb.AppendLine("");

            }





            sb.AppendLine("// Relationships");
            //var dsIliskiliTablolar = SqlSorguCalistir(@"select (select name from sys.tables where object_id=fk.referenced_object_id) as TABLE_NAME,c.name as COLUMN_NAME
            //from sys.foreign_key_columns as fk
            //	inner join sys.tables as t on fk.parent_object_id = t.object_id
            //	inner join sys.columns as c on fk.parent_object_id = c.object_id and fk.parent_column_id = c.column_id
            //where fk.parent_object_id = (select object_id from sys.tables where name = '" + tabloAdi + "' and SCHEMA_NAME(schema_id) = '" + _sema + "') ");

            var dsIliskiliTablolar = SqlSorguCalistir(@" SELECT  c_pk.table_name as TABLE_NAME,  a.column_name as COLUMN_NAME
                                                     FROM all_cons_columns a
                                                     JOIN all_constraints c ON a.owner = c.owner AND a.constraint_name = c.constraint_name
                                                     JOIN all_constraints c_pk ON c.r_owner = c_pk.owner AND c.r_constraint_name = c_pk.constraint_name
                                                     WHERE c.constraint_type = 'R' AND a.table_name = '" + tabloAdi + "'");

            foreach (DataRow drKolon in dsIliskiliTablolar.Tables[0].Rows)
            {

                {
                    var type = "HasRequired";
                    if (nullableColums.Contains(drKolon["COLUMN_NAME"].ToString()))
                    {
                        type = "HasOptional";
                    }

                    if (drKolon["TABLE_NAME"].ToString() == tabloAdi)
                        sb.AppendLine($"{type}(t => t.Ust{drKolon["TABLE_NAME"]}Ef)");
                    else
                    {
                        var name = drKolon["COLUMN_NAME"].ToString().Replace("Id", "");
                        if (name != drKolon["TABLE_NAME"].ToString())
                        {
                            name = $"{drKolon["TABLE_NAME"]}{name}";
                        }


                        sb.AppendLine(
                            $"{type}(t => t.{name}Ef)");
                    }
                }

                {
                    if (drKolon["TABLE_NAME"].ToString() == tabloAdi)
                        sb.AppendLine($"  .WithMany(t => t.Alt{tabloAdi}EfCollection)");
                    else
                    {
                        var name = drKolon["COLUMN_NAME"].ToString().Replace("Id", "") + tabloAdi;
                        //if (name != drKolon["TABLE_NAME"].ToString())
                        //{
                        //	name = $"{name}{tabloAdi}";
                        //}
                        sb.AppendLine($"  .WithMany(t => t.{name}EfCollection)");
                    }
                }

                sb.AppendLine($"  .HasForeignKey(d => d.{drKolon["COLUMN_NAME"]})");
                sb.AppendLine("  .WillCascadeOnDelete(false); ");
                sb.AppendLine("");
            }

            sb.AppendLine("     }");
            sb.AppendLine("  }");
            sb.AppendLine("}");
            DosyaYaz(sb.ToString(), DataLayerPath + @"\Concrete\EntityFramework\Mappings", tabloAdi + "Map.cs");

        }

        #endregion

        #region Entities

        private void EntityOlustur(List<KolonBilgi> kolonList, string tabloAdi)
        {
            StringBuilder sb = new StringBuilder();

            var silindi = _silindiKolonuVarmi ? ",IEntitySoftDelete" : "";

            sb.Append($@"using System;
            using OsosOracle.Framework.Entities;
			namespace {ProjeAdi}.Entities.Concrete
			{{
				public class {tabloAdi} :  IEntity{silindi}
				{{");

            foreach (KolonBilgi kolon in kolonList)
            {
                string cSharpTip = OracleToCSharp(GetEnumValue<MyOracleType>(kolon.DataTipi));

                //strin nullable olmaz
                if (kolon.Nullable == "Y" && cSharpTip != "string")
                { cSharpTip += "?"; }
                sb.AppendLine($"        public {cSharpTip} {kolon.KolonAdi} {{ get; set; }}");
            }

            sb.AppendLine("    }");
            sb.AppendLine("}");

            DosyaYaz(sb.ToString(), EntitiesPath + @"\Concrete", tabloAdi + ".cs");

            // return sb.ToString();
        }

        private void DataTableOlustur(string tabloAdi)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($@"using System.Collections.Generic;
using OsosOracle.Framework.DataAccess;
namespace {ProjeAdi}.Entities.ComplexType.{tabloAdi}ComplexTypes
{{
	public class {tabloAdi}DataTable : DataTable
	{{
		public List<{tabloAdi}Detay> {tabloAdi}DetayList {{ get; set; }}
	}}
}}");
            DosyaYaz(sb.ToString(), EntitiesPath + @"\ComplexType\" + tabloAdi + "ComplexTypes", tabloAdi + "DataTable.cs");

        }

        private void AraComplexOlustur(List<KolonBilgi> kolonList, string tabloAdi)
        {
            StringBuilder sb = new StringBuilder();


            sb.Append($@"
using System;
using OsosOracle.Framework.DataAccess.Filter;

	namespace {ProjeAdi}.Entities.ComplexType.{tabloAdi}ComplexTypes
			{{
				public class {tabloAdi}Ara
				{{ 
					public Ara Ara {{ get; set; }}
					public int? KAYITNO {{ get; set; }}
					public string KAYITNOlar {{ get; set; }}
");
            //Tüm kolonları aramama entitisine ekle  
            foreach (KolonBilgi kolon in kolonList)
            {

                if (!SistemKolonlari.Contains(kolon.KolonAdi)) //sadece gerekli kolonlar
                {
                    string cSharpTip = OracleToCSharp(GetEnumValue<MyOracleType>(kolon.DataTipi));
                    if (cSharpTip != "string") //string olmayan diğer tipler araması nullable olabilir
                        cSharpTip += "?";

                    sb.AppendLine($"public {cSharpTip} {kolon.KolonAdi} {{ get; set; }}");
                }
            }

            sb.AppendLine("}");
            sb.AppendLine("}");
            DosyaYaz(sb.ToString(), EntitiesPath + @"\ComplexType\" + tabloAdi + "ComplexTypes", tabloAdi + "Ara.cs");

        }


        private void DetayComplexOlustur(string tabloAdi, List<KolonBilgi> kolonList)
        {
            var sb = new StringBuilder();

            foreach (KolonBilgi kolon in kolonList)
            {
                string cSharpTip = OracleToCSharp(GetEnumValue<MyOracleType>(kolon.DataTipi));

                //strin nullable olmaz
                if (kolon.Nullable == "YES" && cSharpTip != "string")
                { cSharpTip += "?"; }
                sb.AppendLine($"        public {cSharpTip} {kolon.KolonAdi} {{ get; set; }}");
            }



            var code = $@"using System;
using {ProjeAdi}.Entities.Concrete;
namespace {ProjeAdi}.Entities.ComplexType.{tabloAdi}ComplexTypes
{{
	public class {tabloAdi}Detay
	{{
	
		{sb}
	}}

}}";
            DosyaYaz(code, EntitiesPath + @"\ComplexType\" + tabloAdi + "ComplexTypes", tabloAdi + "Detay.cs");

        }

        #endregion

        #region Genel

        private string GetConnectionString()
        {
            // string connString = "DATA SOURCE=localhost:1521/ORCL;PASSWORD=prod9872;USER ID=YONCAPROD";
            return $"Data Source={_dbserverIp}/{_databaseAdi};User Id={_kullaniciAdi};Password={_sifre}";
        }

        private DataSet SqlSorguCalistir(string sorgu)
        {
            var conn = new OracleConnection(GetConnectionString());
            var da = new OracleDataAdapter(sorgu, conn);
            conn.Open();
            var ds = new DataSet();
            da.Fill(ds);
            conn.Close();
            return ds;
        }

        public List<DbTable> GetTableList()
        {
            // DataSet dsTablolar = SqlSorguCalistir($"SELECT ROW_NUMBER() OVER(ORDER BY TABLE_NAME) as ID,TABLE_NAME FROM INFORMATION_SCHEMA.tables WHERE TABLE_TYPE=\'BASE TABLE\' AND TABLE_SCHEMA =\'{_sema}\' ORDER BY TABLE_NAME");
            DataSet dsTablolar = SqlSorguCalistir($"SELECT DISTINCT OWNER, OBJECT_NAME as TABLE_NAME   FROM ALL_OBJECTS WHERE OBJECT_TYPE = 'TABLE' AND OWNER = '{_kullaniciAdi}' order by OBJECT_NAME");


            return dsTablolar.Tables[0].AsEnumerable().Select(dataRow => new DbTable() { Name = dataRow.Field<string>("TABLE_NAME") }).ToList();
        }

        public List<DbTable> GetViewList()
        {
            DataSet dsTablolar = SqlSorguCalistir(
                $"SELECT ROW_NUMBER() OVER(ORDER BY TABLE_NAME) as ID,TABLE_NAME FROM INFORMATION_SCHEMA.tables WHERE  TABLE_TYPE=\'VIEW\' AND TABLE_SCHEMA =\'{_sema}\' ORDER BY TABLE_NAME");


            return dsTablolar.Tables[0].AsEnumerable().Select(dataRow => new DbTable() { Name = dataRow.Field<string>("TABLE_NAME") }).ToList();
        }


        private void DosyaYaz(string icerik, string klasorAdi, string dosyaAdi)
        {
            var path = $@"{ProjeYolu}{ProjeAdi}\{ProjeAdi}.{klasorAdi}";
            var filePath = $@"{path}\{dosyaAdi}";

            if (!Directory.Exists(path)) //klasör yoksa üret
            {
                Directory.CreateDirectory(path);
            }

            StreamWriter dosya = File.CreateText(filePath);// yeni dosya oluştur.
            dosya.Write(icerik);
            dosya.Close();
        }


        public string SqlToKaydetView(SqlDbType sqlType, string colName, string description = "", string tableName = "", string onEk = "", bool enableValidation = true)
        {
            var sb = new StringBuilder($@"       <div class=""form-group"">").AppendLine("");

            if (description == "")
            {
                description = SplitCamelCase(colName).Replace(" Id", "");
            }

            var propName = $"{onEk}.{colName}";
            switch (sqlType)
            {
                case SqlDbType.Bit:
                    sb.Append($@"    <label>@Html.CheckBoxFor(t => t.{propName}) @_T(""{description}"")</label>");
                    break;

                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                case SqlDbType.Date:
                case SqlDbType.Time:
                case SqlDbType.DateTime2:
                    sb.Append($@"    @Html.DateInput(t => t.{propName}, new {{ placeholder =  _T(""{description}"") }})");
                    break;

                case SqlDbType.Decimal:
                case SqlDbType.Money:
                case SqlDbType.SmallMoney:
                case SqlDbType.Float:
                case SqlDbType.Real:
                    sb.Append($@"    @Html.TextBoxFor(t => t.{propName}, new {{ @class = ""form-control decimal"", placeholder = _T(""{description}"") }})");
                    break;

                case SqlDbType.Int:
                case SqlDbType.TinyInt:
                case SqlDbType.SmallInt:
                case SqlDbType.BigInt:

                    if (colName.Contains("KAYITNO"))
                    {
                        string acMethod;

                        if (!_tableList.Exists(t => t.Equals(colName.Replace("Id", "").Replace("Ust", ""))))
                            acMethod = $"{tableName}{colName.Replace("Id", "").Replace("Ust", "").Replace(".", "")}Getir";
                        else
                            acMethod = $"{colName.Replace("Id", "").Replace(".", "")}Getir";

                        sb.Append($@"    @Html.AutoComplete(t => t.{propName}, Enums.AutocompleteFuction.{acMethod}, Enums.AutoCompleteType.List, _T(""{description}"").ToString())");
                        _autocompleteFunctions.AppendLine(acMethod);
                    }
                    else
                    {
                        sb.Append($@"    @Html.TextBoxFor(t => t.{propName}, new {{ @class = ""form-control int"", placeholder = _T(""{description}"") }})");
                    }
                    break;


                default:
                    sb.Append($@"    @Html.TextBoxFor(t => t.{propName}, new {{ @class = ""form-control"", placeholder = _T(""{description}"") }})");
                    break;

            }
            if (enableValidation)
            {
                sb.AppendLine("").AppendLine($"    @Html.ValidationMessageFor(t => t.{propName})");
            }
            sb.AppendLine("").AppendLine("    </div>");
            return sb.ToString();
        }




        public string SqlToIndexView(SqlDbType sqlType, string colName, string description = "", string tableName = "", string onEk = "", bool enableValidation = true)
        {
            var sb = new StringBuilder($@"<div class=""form-group"">").AppendLine("");

            if (description == "")
            {
                description = SplitCamelCase(colName).Replace(" Id", "");
            }

            var propName = $"{onEk}.{colName}";
            switch (sqlType)
            {
                case SqlDbType.Bit:
                    sb.Append($@"    @_T(""{description}"") @Html.YesNoFor(t => t.{propName})");
                    break;

                case SqlDbType.DateTime:
                case SqlDbType.SmallDateTime:
                case SqlDbType.Date:
                case SqlDbType.Time:
                case SqlDbType.DateTime2:
                    sb.Append($@"    @Html.DateInput(t => t.{propName}, new {{ placeholder =  _T(""{description}"") }})");
                    break;

                case SqlDbType.Decimal:
                case SqlDbType.Money:
                case SqlDbType.SmallMoney:
                case SqlDbType.Float:
                case SqlDbType.Real:
                    sb.Append($@"   @Html.TextBoxFor(t => t.{propName}, new {{ @class = ""form-control decimal"", placeholder = _T(""{description}"") }})");
                    break;

                case SqlDbType.Int:
                case SqlDbType.TinyInt:
                case SqlDbType.SmallInt:
                case SqlDbType.BigInt:

                    if (colName.Contains("Id"))
                    {
                        string acMethod;

                        if (!_tableList.Exists(t => t.Equals(colName.Replace("Id", "").Replace("Ust", ""))))
                            acMethod = $"{tableName}{colName.Replace("Id", "").Replace("Ust", "").Replace(".", "")}Getir";
                        else
                            acMethod = $"{colName.Replace("Id", "").Replace(".", "")}Getir";

                        sb.Append($@"    @Html.AutoComplete(t => t.{propName}, Enums.AutocompleteFuction.{acMethod}, Enums.AutoCompleteType.List, _T(""{description}"").ToString())");
                    }

                    else
                    {
                        sb.Append($@"    @Html.TextBoxFor(t => t.{propName}, new {{ @class = ""form-control int"", placeholder = _T(""{description}"") }})");
                    }
                    break;


                default:
                    sb.Append($@"    @Html.TextBoxFor(t => t.{propName}, new {{ @class = ""form-control"", placeholder = _T(""{description}"") }})");
                    break;

            }
            if (enableValidation)
            {
                sb.AppendLine("").AppendLine($"   @Html.ValidationMessageFor(t => t.{propName})");
            }
            sb.AppendLine("").AppendLine("</div>");
            return sb.ToString();
        }





        public string SqlToDisplay(SqlDbType sqlType, string colName, string description = "", string tableName = "", string onEk = "")
        {

            if (description == "")
            {
                description = SplitCamelCase(colName).Replace(" Id", "");
            }


            var
            sb = new StringBuilder($@"
	<div class=""row static-info"">
		<div class=""col-md-5 name"">@_T(""{description}"")</div>
		<div class=""col-md-7 value"">@Model.{onEk}.{colName}</div>
	 </div>
");


            return sb.ToString();
        }





        public static string OracleToCSharp(MyOracleType oracleType)
        {
            switch (oracleType)
            {
                case MyOracleType.Char:
                case MyOracleType.NChar:
                case MyOracleType.Varchar2:
                case MyOracleType.NVarchar2:
                case MyOracleType.Clob:
                    return "string";
                case MyOracleType.Decimal:
                case MyOracleType.Int16:
                case MyOracleType.Int32:
                case MyOracleType.Number:
                    return "int";
                case MyOracleType.Int64:
                case MyOracleType.Double:
                    return "long";
                case MyOracleType.Date:
                    return "DateTime";
                case MyOracleType.BFile:
                case MyOracleType.Blob:
                case MyOracleType.Raw:
                    return "Byte[]";


                default:
                    throw new ArgumentOutOfRangeException("oracleType");
            }
        }





        public T GetEnumValue<T>(string str) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("T must be an Enumeration type.");
            }
            T val = ((T[])Enum.GetValues(typeof(T)))[0];
            if (!string.IsNullOrEmpty(str))
            {

                foreach (T enumValue in (T[])Enum.GetValues(typeof(T)))
                {

                    if (enumValue.ToString().ToUpper().Replace("İ", "I").Equals(str.ToUpper().Replace("İ", "I")))
                    {
                        val = enumValue;
                        break;
                    }


                }
            }

            return val;
        }


        public string IlkHarfiKucukYap(string str)
        {
            return str.Substring(0, 1).ToLower() + str.Substring(1, str.Length - 1);
        }

        public string SplitCamelCase(string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }


        private bool SilindiKolonuVarmi(List<KolonBilgi> kolonlar)
        {

            return kolonlar.Exists(t => t.KolonAdi == "Silindi");

        }

        private void CompressFolder(string path, ZipOutputStream zipStream, int folderOffset)
        {

            string[] files = Directory.GetFiles(path);

            foreach (var filename in files)
            {

                var fi = new FileInfo(filename);

                string entryName = filename.Substring(folderOffset); // Makes the name in zip based on the folder
                entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
                var newEntry = new ZipEntry(entryName)
                {
                    DateTime = fi.LastWriteTime,
                    Size = fi.Length
                };
                // Note the zip format stores 2 second granularity

                // Specifying the AESKeySize triggers AES encryption. Allowable values are 0 (off), 128 or 256.
                // A password on the ZipOutputStream is required if using AES.
                //   newEntry.AESKeySize = 256;

                // To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
                // you need to do one of the following: Specify UseZip64.Off, or set the Size.
                // If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
                // but the zip will be in Zip64 format which not all utilities can understand.
                //   zipStream.UseZip64 = UseZip64.Off;

                zipStream.PutNextEntry(newEntry);

                // Zip the file in buffered chunks
                // the "using" will close the stream even if an exception occurs
                byte[] buffer = new byte[4096];
                using (FileStream streamReader = File.OpenRead(filename))
                {
                    StreamUtils.Copy(streamReader, zipStream, buffer);
                }
                zipStream.CloseEntry();
            }
            string[] folders = Directory.GetDirectories(path);
            foreach (var folder in folders)
            {
                CompressFolder(folder, zipStream, folderOffset);
            }
        }


        #endregion


    }

    public class KolonBilgi
    {
        public string KolonAdi { get; set; }
        public string Nullable { get; set; }
        public string DataTipi { get; set; }
        public string MaxLength { get; set; }
    }

    public class DbTable
    {
        public string Name { get; set; }
    }
}
