using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminario.Datos.Type.ResultType
{
    public class ResultType
    {
        public ResultType()
        {
            hayErrores = false;
            errores = new Collection<string>();
            isSessionAlive = true;
        }

        public ResultType(object data)
        {
            this.data = data;
            hayErrores = false;
        }

        /// <summary>
        ///     Campo donde devolvemos la informacion.
        /// </summary>
        public object data { get; set; }

        /// <summary>
        ///     Campo donde indica si tenemos errores
        /// </summary>
        public bool hayErrores { get; private set; }

        /// <summary>
        ///     Campo donde mostramos el detalle del error
        /// </summary>
        public string error { get; private set; }

        public bool isSessionAlive { get; set; }
        public ICollection<string> errores { get; set; }
        public bool ok => !hayErrores;

        public void setError(string mensaje)
        {
            data = null;
            error = mensaje;
            hayErrores = true;
        }

        public void setData(object obj)
        {
            data = obj;
            hayErrores = false;
            errores = new Collection<string>();
            isSessionAlive = true;
        }

        public void AgregarError(IEnumerable<string> errors)
        {
            foreach (var item in errors)
            {
                AgregarError(item);
            }
        }

        public void AgregarError(string errorMessage)
        {
            error = errorMessage;
            errores.Add(error);
            hayErrores = true;
        }

        public void setErrorCode(string errorCode)
        {
            data = errorCode;
        }
    }
}
