﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PetCenter.Referencias.Presentacion.Web.Resources {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Mensajes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Mensajes() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PetCenter.Referencias.Presentacion.Web.Resources.Mensajes", typeof(Mensajes).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Ocurrió un error en: {0} - {1}, favor de revisar Log..
        /// </summary>
        public static string ErrorGenerico {
            get {
                return ResourceManager.GetString("ErrorGenerico", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a No se encontró información para los criterios ingresados.
        /// </summary>
        public static string NoSeEncontroInformacion {
            get {
                return ResourceManager.GetString("NoSeEncontroInformacion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a No se pudo grabar la solicitud..
        /// </summary>
        public static string SolicitudError {
            get {
                return ResourceManager.GetString("SolicitudError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Se generó el número de solicitud: {0}.
        /// </summary>
        public static string SolicitudRegistrada {
            get {
                return ResourceManager.GetString("SolicitudRegistrada", resourceCulture);
            }
        }
    }
}
