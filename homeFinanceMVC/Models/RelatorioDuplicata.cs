using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using homeFinanceMVC.Models;

namespace homeFinanceMVC.Models
{
    public class RelatorioDuplicata : TNEReport
    {
        

        public RelatorioDuplicata()
        {
            Paisagem = false;
        }

        public override void MontaCorpoDados()
        {
            CuentaDAO cDAO = new CuentaDAO();
            UsuarioDAO uDAO = new UsuarioDAO();

            base.MontaCorpoDados();

            #region Cabeçalho do Relatório
            PdfPTable table = new PdfPTable(2);
            BaseColor preto = new BaseColor(0, 0, 0);
            BaseColor fundo = new BaseColor(200, 200, 200);
            Font font = FontFactory.GetFont("Verdana", 8, Font.NORMAL, preto);
            Font titulo = FontFactory.GetFont("Verdana", 8, Font.BOLD, preto);

            float[] colsW = { 10, 10 };
            table.SetWidths(colsW);
            table.HeaderRows = 1;
            table.WidthPercentage = 100f;

            table.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
            table.DefaultCell.BorderColor = preto;
            table.DefaultCell.BorderColorBottom = new BaseColor(255, 255, 255);
            table.DefaultCell.Padding = 10;

            table.AddCell(getNewCell("Descripción", titulo, Element.ALIGN_LEFT, 10, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(getNewCell("Usuario", titulo, Element.ALIGN_LEFT, 10, PdfPCell.BOTTOM_BORDER, preto, fundo));
            #endregion

            var duplicatas = cDAO.ListarCuentas();
            var clienteOld = string.Empty;

            foreach (var d in duplicatas)
            {
                //if (d.cliente.RazaoSocial != clienteOld)
                //{
                //    var cell = getNewCell(d.cliente.RazaoSocial, titulo, Element.ALIGN_LEFT, 10, PdfPCell.BOTTOM_BORDER);
                //    cell.Colspan = 5;
                //    table.AddCell(cell);
                //    clienteOld = d.cliente.RazaoSocial;
                //}

                table.AddCell(getNewCell(d.DescCuenta, font, Element.ALIGN_LEFT, 5, PdfPCell.BOTTOM_BORDER));
                table.AddCell(getNewCell(uDAO.ObtenerUsuarioPorId(d.IdUsuario).Nombre, font, Element.ALIGN_LEFT, 5, PdfPCell.BOTTOM_BORDER));
            }

            doc.Add(table);
        }


        public override void MontaCorpoDadosUsuario()
        {
            CuentaDAO cDAO = new CuentaDAO();
            UsuarioDAO uDAO = new UsuarioDAO();


            base.MontaCorpoDadosUsuario();

            #region Cabeçalho do Relatório
            PdfPTable table = new PdfPTable(5);
            BaseColor preto = new BaseColor(0, 0, 0);
            BaseColor fundo = new BaseColor(200, 200, 200);
            Font font = FontFactory.GetFont("Verdana", 8, Font.NORMAL, preto);
            Font titulo = FontFactory.GetFont("Verdana", 8, Font.BOLD, preto);

            float[] colsW = { 10, 10, 10, 10, 10 };
            table.SetWidths(colsW);
            table.HeaderRows = 1;
            table.WidthPercentage = 100f;

            table.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
            table.DefaultCell.BorderColor = preto;
            table.DefaultCell.BorderColorBottom = new BaseColor(255, 255, 255);
            table.DefaultCell.Padding = 10;

            table.AddCell(getNewCell("Login", titulo, Element.ALIGN_LEFT, 10, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(getNewCell("Nombre", titulo, Element.ALIGN_LEFT, 10, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(getNewCell("Correo Eletrónico", titulo, Element.ALIGN_LEFT, 10, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(getNewCell("Tipo", titulo, Element.ALIGN_LEFT, 10, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(getNewCell("Situación", titulo, Element.ALIGN_LEFT, 10, PdfPCell.BOTTOM_BORDER, preto, fundo));
            #endregion

            var usuarios = uDAO.ListarUsuarios();
            var clienteOld = string.Empty;

            string descTipo = "";
            string descSit = "";

            foreach (var u in usuarios)
            {
                if (u.Tipo == 1)
                {
                    descTipo = "Administrador";
                }
                else
                {
                    descTipo = "Usuario";
                }

                if (u.Situacion == 0)
                {
                    descSit = "Inactivo";
                }
                else
                {
                    descSit = "Activo";
                }
                table.AddCell(getNewCell(u.Login, font, Element.ALIGN_LEFT, 5, PdfPCell.BOTTOM_BORDER));
                table.AddCell(getNewCell(u.Nombre, font, Element.ALIGN_LEFT, 5, PdfPCell.BOTTOM_BORDER));
                table.AddCell(getNewCell(u.Email, font, Element.ALIGN_LEFT, 5, PdfPCell.BOTTOM_BORDER));
                table.AddCell(getNewCell(descTipo, font, Element.ALIGN_RIGHT, 5, PdfPCell.BOTTOM_BORDER));
                table.AddCell(getNewCell(descSit, font, Element.ALIGN_RIGHT, 5, PdfPCell.BOTTOM_BORDER));
            }

            doc.Add(table);
        }

        public override void MontaCorpoDadosEvento()
        {
            EvActivoPasivoDAO evDAO = new EvActivoPasivoDAO();


            base.MontaCorpoDadosEvento();

            #region Cabeçalho do Relatório
            PdfPTable table = new PdfPTable(2);
            BaseColor preto = new BaseColor(0, 0, 0);
            BaseColor fundo = new BaseColor(200, 200, 200);
            Font font = FontFactory.GetFont("Verdana", 8, Font.NORMAL, preto);
            Font titulo = FontFactory.GetFont("Verdana", 8, Font.BOLD, preto);

            float[] colsW = { 10, 10 };
            table.SetWidths(colsW);
            table.HeaderRows = 1;
            table.WidthPercentage = 100f;

            table.DefaultCell.Border = PdfPCell.BOTTOM_BORDER;
            table.DefaultCell.BorderColor = preto;
            table.DefaultCell.BorderColorBottom = new BaseColor(255, 255, 255);
            table.DefaultCell.Padding = 10;

            table.AddCell(getNewCell("Descripción", titulo, Element.ALIGN_LEFT, 10, PdfPCell.BOTTOM_BORDER, preto, fundo));
            table.AddCell(getNewCell("Tipo", titulo, Element.ALIGN_LEFT, 10, PdfPCell.BOTTOM_BORDER, preto, fundo));
            #endregion

            var eventos = evDAO.ListarTodosEventos();
            var clienteOld = string.Empty;

            string descEv = "";

            foreach (var ev in eventos)
            {
                if (ev.TipoEvento == "C")
                {
                    descEv = "Credito";
                }
                else
                {
                    descEv = "Debito";
                }
                table.AddCell(getNewCell(ev.DescEvento, font, Element.ALIGN_LEFT, 5, PdfPCell.BOTTOM_BORDER));
                table.AddCell(getNewCell(descEv, font, Element.ALIGN_LEFT, 5, PdfPCell.BOTTOM_BORDER));
            }

            doc.Add(table);
        }




    }
}

