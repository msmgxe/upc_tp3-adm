﻿using PetCenter.Referencias.Dominio.Administracion.Base;
using PetCenter.Referencias.Dominio.Administracion.DTOs.Comun;
using PetCenter.Referencias.Dominio.Administracion.DTOs.Registros.Solicitud;
using PetCenter.Referencias.Presentacion.Web.Controllers.Comun;
using PetCenter.Referencias.Presentacion.Web.Helpers.Extensiones;
using PetCenter.Referencias.Presentacion.Web.Helpers.Mvc;
using PetCenter.Referencias.Presentacion.Web.Models.Registros.Evaluacion;
using PetCenter.Referencias.Presentacion.Web.Models.Registros.Solicitud;
using PetCenter.Referencias.Presentacion.Web.Resources;
using PetCenter.Referencias.Presentacion.Web.Resources.Enumeraciones;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace PetCenter.Referencias.Presentacion.Web.Controllers.Registros.Evaluacion
{
    public class EvaluacionController : BaseController
    {
        #region BANDEJA

        public ActionResult Index(int page = 1,
                                       string sort = "NroSolicitud",
                                       string sortDir = "DESC",
                                       EvaluacionPaginadoModelo tablaPaginado = null,
                                       string mensaje = null,
                                       bool back = false
                                       )
        {
            try
            {

                //if (back) tablaPaginado = GetCache(tablaPaginado);

                //Asignamos valores iniciales
                tablaPaginado = IniciarFiltro(tablaPaginado);

                //Construimos solicitud
                var solicitud = ConstruirSolicitud(page, sort, sortDir, tablaPaginado);

                //Invocamos al servicio            
                var respuesta = _registrosServicio.SolicitudServicio.Busqueda(solicitud);

                //construimos modelo
                var model = ConstruirModeloPaginado(page, respuesta, tablaPaginado.Filtro);
                model.AsignarMensaje(mensaje);

                //if (!back) SetCache(tablaPaginado);
                return View("_Index", model);
            }
            catch (Exception ex)
            {
                var respuesta = new RespuestaSolicitudDto();
                respuesta.lista = new List<SolicitudDto>();
                respuesta.TotalElementos = 0;
                var model = ConstruirModeloPaginado(page, respuesta, tablaPaginado.Filtro);
                mensaje = MensajeMvc.MensajePeligro(Mensajes.ErrorGenerico, Util.ObtenerControllerName(Request), Util.ObtenerActionName(Request));
                model.AsignarMensaje(mensaje);
                return View("_Index", model);
            }
        }

        #endregion

        #region NUEVO

        // GET: Solicitud
        public ActionResult Nuevo()
        {

            var modelo = new SolicitudEditorModelo
            {
                Solicitud = new SolicitudDto() { FechaSolicitud = DateTime.Now, FechaVencimientoG = DateTime.Now }
            };

            ViewBag.TiposDocumento = _maestrosServicio.GeneralServicio.TipoDocumentoListar();
            ViewBag.Monedas = _maestrosServicio.MonedaServicio.Listar();
            ViewBag.Bancos = _maestrosServicio.BancoServicio.Listar();
            return View("_Registrar", modelo);
        }

        //[HttpPost]
        public ActionResult Registrar(RegistrarSolicitudDto modelo)
        {

            string mensaje = string.Empty;

            var fileStreamrs = TempData["fileStreamrs"] as HttpPostedFileBase;
            var fileStreamccv = TempData["fileStreamccv"] as HttpPostedFileBase;
            var fileStreamrsu = TempData["fileStreamrsu"] as HttpPostedFileBase;
            var fileStreamlf = TempData["fileStreamlf"] as HttpPostedFileBase;
            var fileStreamrcr = TempData["fileStreamrcr"] as HttpPostedFileBase;

            if (fileStreamrs != null)
            {
                modelo.Solicitud.DocReciboObjeto = fileStreamrs.InputStream.ObtenerBytesOfStream();
                modelo.Solicitud.DocReciboTitulo = fileStreamrs.FileName;
            }
            if (fileStreamccv != null)
            {
                modelo.Solicitud.DocColegiaturaObjeto = fileStreamccv.InputStream.ObtenerBytesOfStream();
                modelo.Solicitud.DocColegiaturaTitulo = fileStreamccv.FileName;
            }
            if (fileStreamrsu != null)
            {
                modelo.Solicitud.DocSunatObjeto = fileStreamrsu.InputStream.ObtenerBytesOfStream();
                modelo.Solicitud.DocSunatTitulo = fileStreamrsu.FileName;
            }
            if (fileStreamlf != null)
            {
                modelo.Solicitud.DocLicenciaObjeto = fileStreamlf.InputStream.ObtenerBytesOfStream();
                modelo.Solicitud.DocLicenciaTitulo = fileStreamlf.FileName;
            }
            if (fileStreamrcr != null)
            {
                modelo.Solicitud.DocCentralObjeto = fileStreamrcr.InputStream.ObtenerBytesOfStream();
                modelo.Solicitud.DocCentralTitulo = fileStreamrcr.FileName;
            }

            TempData["fileStreamccv"] = null;
            TempData["fileStreamccv"] = null;
            TempData["fileStreamrsu"] = null;
            TempData["fileStreamlf"] = null;
            TempData["fileStreamrcr"] = null;

            //Registra el área
            var resultado = _registrosServicio.SolicitudServicio.Registrar(modelo);
            if (resultado == -1)
                mensaje = MensajeMvc.MensajeError(Mensajes.SolicitudError);
            else
                mensaje = MensajeMvc.MensajeSatisfactorio(string.Format(Mensajes.SolicitudRegistrada, resultado));

            return RedirectToAction("Index", "Evaluacion", new { mensaje = mensaje, back = true });

        }

        #endregion

        #region MODIFICAR

        /// <summary>
        /// Modificar Solicitud
        /// </summary>
        /// <param name="CodigoSolicitud">int</param>
        /// <returns>ActionResult</returns>
        public ActionResult Editar(int idSolicitud)
        {
            var mensaje = string.Empty;

            try
            {
                //Generamos la solicitud
                var solicitud = new RegistrarSolicitudDto
                {
                    Solicitud = _registrosServicio.SolicitudServicio.Buscar(idSolicitud)
                };

                if (solicitud.Solicitud == null) solicitud = null;

                //Construimos el modelo
                var modelo = new SolicitudEditorModelo
                {
                    Solicitud = solicitud.Solicitud,
                };

                ViewBag.TiposDocumento = _maestrosServicio.GeneralServicio.TipoDocumentoListar();
                ViewBag.Monedas = _maestrosServicio.MonedaServicio.Listar();
                ViewBag.Bancos = _maestrosServicio.BancoServicio.Listar();

                return View("_Modificar", modelo);
            }
            catch (Exception)
            {
                mensaje = MensajeMvc.MensajePeligro(Mensajes.ErrorGenerico, Util.ObtenerControllerName(Request), Util.ObtenerActionName(Request));
                var modelo = new SolicitudEditorModelo
                {
                    Solicitud = new SolicitudDto()
                };
                modelo.AsignarMensaje(mensaje);
                return View("_Index", modelo);
            }
        }

        /// <summary>
        /// Modificar
        /// </summary>
        /// <param name="modelo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Modificar(SolicitudEditorModelo modelo)
        {
            var mensaje = string.Empty;

            try
            {
                var solicitud = new RegistrarSolicitudDto
                {
                    Solicitud = modelo.Solicitud
                };

                var fileStreamrs = TempData["fileStreamrs"] as HttpPostedFileBase;
                var fileStreamccv = TempData["fileStreamccv"] as HttpPostedFileBase;
                var fileStreamrsu = TempData["fileStreamrsu"] as HttpPostedFileBase;
                var fileStreamlf = TempData["fileStreamlf"] as HttpPostedFileBase;
                var fileStreamrcr = TempData["fileStreamrcr"] as HttpPostedFileBase;

                if (fileStreamrs != null)
                {
                    modelo.Solicitud.DocReciboObjeto = fileStreamrs.InputStream.ObtenerBytesOfStream();
                    modelo.Solicitud.DocReciboTitulo = fileStreamrs.FileName;
                }
                if (fileStreamccv != null)
                {
                    modelo.Solicitud.DocColegiaturaObjeto = fileStreamccv.InputStream.ObtenerBytesOfStream();
                    modelo.Solicitud.DocColegiaturaTitulo = fileStreamccv.FileName;
                }
                if (fileStreamrsu != null)
                {
                    modelo.Solicitud.DocSunatObjeto = fileStreamrsu.InputStream.ObtenerBytesOfStream();
                    modelo.Solicitud.DocSunatTitulo = fileStreamrsu.FileName;
                }
                if (fileStreamlf != null)
                {
                    modelo.Solicitud.DocLicenciaObjeto = fileStreamlf.InputStream.ObtenerBytesOfStream();
                    modelo.Solicitud.DocLicenciaTitulo = fileStreamlf.FileName;
                }
                if (fileStreamrcr != null)
                {
                    modelo.Solicitud.DocCentralObjeto = fileStreamrcr.InputStream.ObtenerBytesOfStream();
                    modelo.Solicitud.DocCentralTitulo = fileStreamrcr.FileName;
                }

                TempData["fileStreamccv"] = null;
                TempData["fileStreamccv"] = null;
                TempData["fileStreamrsu"] = null;
                TempData["fileStreamlf"] = null;
                TempData["fileStreamrcr"] = null;

                var resultado = _registrosServicio.SolicitudServicio.Modificar(solicitud);
                if (resultado == -1)
                    mensaje = MensajeMvc.MensajePeligro(Mensajes.ErrorGenerico, Util.ObtenerControllerName(Request), Util.ObtenerActionName(Request));
                else
                    mensaje = MensajeMvc.MensajeSatisfactorio(Mensajes.SolicitudModificada, modelo.Solicitud.NroSolicitud);
            }
            catch (Exception ex)
            {
                mensaje = MensajeMvc.MensajePeligro(Mensajes.ErrorGenerico, ex.Message);
            }

            return RedirectToAction("Index", "Evaluacion", new { mensaje = mensaje, back = true });
        }

        #endregion

        #region VER
        public ActionResult Ver(int idSolicitud)
        {
            var mensaje = string.Empty;

            try
            {
                //Generamos la solicitud
                var solicitud = new RegistrarSolicitudDto
                {
                    Solicitud = _registrosServicio.SolicitudServicio.Buscar(idSolicitud)
                };

                if (solicitud.Solicitud == null) solicitud = null;

                //Construimos el modelo
                var modelo = new EvaluacionEditorModelo
                {
                    Solicitud = solicitud.Solicitud,
                };

                ViewBag.TiposDocumento = _maestrosServicio.GeneralServicio.TipoDocumentoListar();
                ViewBag.Monedas = _maestrosServicio.MonedaServicio.Listar();
                ViewBag.Bancos = _maestrosServicio.BancoServicio.Listar();

                return View("_Mostrar", modelo);
            }
            catch (Exception ex)
            {
                mensaje = MensajeMvc.MensajePeligro(Mensajes.ErrorGenerico, ex.Message);
                var modelo = new EvaluacionEditorModelo
                {
                    Solicitud = new SolicitudDto()
                };
                modelo.AsignarMensaje(mensaje);
                return View("_Index", modelo);
            }
        }
        #endregion

        #region DESCARGA ARCHIVO
        [HttpGet]
        public ActionResult Descarga(int idSolicitud, int identificador)
        {
            var solicitud = _registrosServicio.SolicitudServicio.Buscar(idSolicitud);
            var nombreArchivo = "";
            byte[] content = null;
            if (identificador == 1)
            {
                content = solicitud.DocReciboObjeto;
                nombreArchivo = solicitud.DocReciboTitulo;
            }
            else if (identificador == 2)
            {
                content = solicitud.DocColegiaturaObjeto;
                nombreArchivo = solicitud.DocColegiaturaTitulo;
            }
            else if (identificador == 3)
            {
                content = solicitud.DocSunatObjeto;
                nombreArchivo = solicitud.DocSunatTitulo;
            }
            else if (identificador == 4)
            {
                content = solicitud.DocLicenciaObjeto;
                nombreArchivo = solicitud.DocLicenciaTitulo;
            }
            else if (identificador == 5)
            {
                content = solicitud.DocCentralObjeto;
                nombreArchivo = solicitud.DocCentralTitulo;
            }

            try
            {
                var extension = nombreArchivo.Split('.').Length;

                return File(content, nombreArchivo.Split('.')[extension - 1].ContentTypeExtension(), nombreArchivo);
            }
            catch (Exception ex)
            {
                //Retornando excepción en JSON
                return Json(MensajeMvc.MensajeJson(TipoNotificacionEnum.Advertencia, ex.Message));
            }
        }
        #endregion

        #region MÉTODOS - APOYO

        /// <summary>
        /// ConstruirModeloPaginado
        /// </summary>
        /// <param name="pagina">int</param>
        /// <param name="respuesta">RespuestaSolicitudDto</param>
        /// <param name="filtro">SolicitudFiltroModelo</param>
        /// <returns>SolicitudPaginadoModelo</returns>
        internal EvaluacionPaginadoModelo ConstruirModeloPaginado(int pagina, RespuestaSolicitudDto respuesta, EvaluacionFiltroModelo filtro)
        {
            return new EvaluacionPaginadoModelo
            {
                Grid = new EvaluacionGridModelo(respuesta.lista, Convert.ToInt32(Paginacion.TamanioPagina10), respuesta.TotalElementos),
                Filtro = new EvaluacionFiltroModelo(filtro.Solicitud)
            };
        }

        /// <summary>
        /// ConstruirSolicitud
        /// </summary>
        /// <param name="pagina">int</param>
        /// <param name="orden">string</param>
        /// <param name="ordernDir">string</param>
        /// <param name="tablaPaginado">SolicitudPaginadoModelo</param>
        /// <returns>BusquedaSolicitudDto</returns>
        internal BusquedaSolicitudDto ConstruirSolicitud(int pagina, string orden, string ordernDir, EvaluacionPaginadoModelo tablaPaginado)
        {
            return new BusquedaSolicitudDto
            {
                TablaFilter = tablaPaginado.Filtro.Solicitud,
                CriterioPaginar = new CriterioPaginarDto
                {
                    Tamanio = Convert.ToInt32(Paginacion.TamanioPagina10),
                    Pagina = pagina,
                    Orden = orden,
                    OrdenDir = ordernDir
                }
            };
        }

        /// <summary>
        /// IniciarFiltro
        /// </summary>
        /// <param name="tablaPaginado">TablaTablaPaginadoModelo</param>
        /// <returns></returns>
        internal EvaluacionPaginadoModelo IniciarFiltro(EvaluacionPaginadoModelo SolicitudPaginado)
        {
            if (SolicitudPaginado == null) SolicitudPaginado = new EvaluacionPaginadoModelo();
            if (SolicitudPaginado.Filtro.Solicitud == null)
            {
                SolicitudPaginado.Filtro.Solicitud = new SolicitudDto();
                SolicitudPaginado.Filtro.Solicitud.FechaSolicitudInicio = DateTime.Now.AddDays(-30);
                SolicitudPaginado.Filtro.Solicitud.FechaSolicitudHasta = DateTime.Now;
            }
            else
            {

                if (SolicitudPaginado.Filtro.Solicitud.FechaSolicitudInicio.ToShortDateString() == "1/01/0001")
                {
                    SolicitudPaginado.Filtro.Solicitud.FechaSolicitudInicio = DateTime.Now;
                }

                if (SolicitudPaginado.Filtro.Solicitud.FechaSolicitudHasta.ToShortDateString() == "1/01/0001")
                {
                    SolicitudPaginado.Filtro.Solicitud.FechaSolicitudHasta = DateTime.Now.AddDays(-30);
                }
            }

            return SolicitudPaginado;
        }

        #endregion
    }
}