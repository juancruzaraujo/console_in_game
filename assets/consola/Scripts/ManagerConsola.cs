using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace InGameConsole
{
    public  class ManagerConsola : MonoBehaviour
    {


        #if DEBUG
            private /*static*/ bool modo_Debug = true;
            private const int C_MAX_LINEAS = 17; //esto sirve para el tipo de fuente que tengo, con arial cambia a 15
       #else
            private /*static*/ bool modo_Debug = false;
            private const int C_MAX_LINEAS = 34;
        #endif

        //new Color32(50, 50, 50, 255);
        //public const Color32(50, 50, 50, 255) C_DEFAULT_COLORR;

    

        public static ManagerConsola instance;
        public KeyCode ConsoleOpenKey;
        //public Sprite imgFondo;

        //objetos de la consola

        //[SerializeField]
        private Image _backGroundLogImg;

        //[SerializeField]
        public Sprite _backGroundLogSprite;

        [SerializeField]
        private Font _LogFont;
        [SerializeField]
        private int _LogFontSize;

        private Canvas CanvasConsola;
        private Text txtLogConsola;
        private InputField txtImputConsola;
        private bool _ConsolaAbierta;

        private int _PosInicio = 0;
        private int _PosFinal = 0;
        private int _PosFinalHastaDondeMuestro;
        private List<string> _LineaIngresada; //va a guardar las lineas ingresadas para poder mostrar siempre la ultima línea
        private List<string> _ComandoIngresado;
        //private List<string> _LogLinea; //guarda la linea ingresada en el log, así uno se puede desplazar por el texto

        private string _Comando;
        private string _Parametros;
        private string _Resultado;
        private int _nIndiceComando;

        //variables que van a contener el valor de propiedades
        private bool _ShowSintaxisError; //muestra error si el comando no existe
        private bool _PauseGame; //pausa el juego si la consola se abre

        

        #region Propiedades

        public Sprite LogBackGroundImage
        {
            set
            {
                _backGroundLogSprite = value;
                _backGroundLogImg.sprite = _backGroundLogSprite;

            }
            get
            {
                _backGroundLogImg.sprite = _backGroundLogSprite;
                return _backGroundLogSprite;
            }
        }


        /// <summary>
        /// If the command does not exist it shows an error
        /// </summary>
        /// 
        public bool ShowSintaxError
        {
            set
            {
                _ShowSintaxisError = value;
            }
            get
            {
                return _ShowSintaxisError;
            }
        }

        /// <summary>
        /// Pause the game if the console opens
        /// </summary>
        public bool PauseGame
        {
            set
            {
                _PauseGame = value;
            }
            get
            {
                return _PauseGame;
            }
        }

        public Font LogFont
        {
            set
            {
                _LogFont = value;
                txtLogConsola.font = _LogFont;
            }
            get
            {
                return _LogFont;
            }
        }

        public int LogFontSize
        {
            set
            {
                if (value > 0)
                {
                    _LogFontSize = value;
                    txtLogConsola.fontSize = _LogFontSize;
                }
            }
            get
            {
                return _LogFontSize;
            }
        }
        /*
        public Font InputFont
        {
            set
            {
                _InputFont = value;
                txtImputConsola.textComponent.font = _InputFont;
            }
            get
            {
                return _InputFont;
            }
        }

        public int InputFontSize
        {
            set
            {
                if (value > 0)
                {
                    _InputFontSize = value;
                    txtImputConsola.textComponent.resizeTextForBestFit = true;
                    txtImputConsola.textComponent.fontSize = _InputFontSize;
                }
            }
            get
            {
                return _InputFontSize;
            }
        }
        */
        #endregion

        void Awake()
        {

            if (instance == null)
            {
                instance = this;

                SetConsolaInicial();
            }
            else
            {
                Destroy(this);
            }

        }

        private void SetConsolaInicial()
        {
            CanvasConsola = GameObject.Find("CanvasConsole").GetComponent<Canvas>();

            txtImputConsola = GameObject.Find("InputTextConsole").GetComponent<InputField>();
            txtLogConsola = GameObject.Find("txtLogConsole").GetComponent<Text>();

            _backGroundLogImg = GameObject.Find("imgBack").GetComponent<Image>();
            _backGroundLogSprite = LogBackGroundImage;
            _backGroundLogImg.sprite = _backGroundLogSprite;

            _LineaIngresada = new List<string>();
            _ComandoIngresado = new List<string>();

            _ConsolaAbierta = false;
            SetValoresVisualizcionConsola();

            if (_LogFont != null)
            {
                txtLogConsola.font = _LogFont;
            }

            if (_LogFontSize != 0)
            {
                txtLogConsola.fontSize = _LogFontSize;
            } 

        }

        void Start()
        {

            _backGroundLogImg.sprite = _backGroundLogSprite;
            //esto nunca funciono, averiguar bien como se hace y no borrar estes comentarios
            //Font miFuente = (Font)Resources.Load("Assets/Consola/Arte/Fuentes/Ubuntu_Mono/UbuntuMono-R_1.ttf", typeof(Font));
            //txtLogConsola.font = miFuente;
            //if (_comandosIniciados == false)
            //{
            //Iniciar_Comandos();
            //}


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="backColor"></param>
        public void BackColor(Color32 backColor)
        {
            _backGroundLogImg.color = backColor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fontColor"></param>
        public void FontColor(Color32 fontColor)
        {
            txtLogConsola.color = fontColor;
            txtImputConsola.textComponent.color = fontColor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fontColor"></param>
        public void CommandFontColor(Color32 fontColor)
        {
            txtImputConsola.textComponent.color = fontColor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fontColor"></param>
        public void LogFontColor(Color32 fontColor)
        {
            txtLogConsola.color = fontColor;
        }

        /// <summary>
        /// Escribe una línea en el log de la consola
        /// </summary>
        /// <param name="Dato">el dato a mostrar en la consola</param>
        public void WriteLine(string Data)
        {
            int nPosMostrar;

            if (_LineaIngresada != null)
            {
                _LineaIngresada.Add(Data);
            }

            
            if (_LineaIngresada.Count > C_MAX_LINEAS)
            {
                txtLogConsola.text = "";
                nPosMostrar = _LineaIngresada.Count - C_MAX_LINEAS; //15 es el máximo de líneas que veo (en tiempo de desarrollo)
                for (int Indice = nPosMostrar; Indice < _LineaIngresada.Count; Indice++)
                {
                    txtLogConsola.text = txtLogConsola.text + _LineaIngresada[Indice] + "\r\n";
                }
            }
            else
            {
                //txtLogConsola.text = txtLogConsola.text + Data + "\r\n";
                txtLogConsola.text = txtLogConsola.text + _LineaIngresada[_LineaIngresada.Count-1] + "\r\n";
            }

            _PosFinalHastaDondeMuestro = _LineaIngresada.Count - 1; //me guardo hasta donde muestro

        }


        public string FormatWrite(string text,string Color,bool Bold, bool Italic)
        {
            if (Bold == true)
            {
                text = "<b>" + text + "</b>";
            }
            if (Italic==true)
            {
                text = "<i>" + text + "</i>";
            }

            if (Color !="")
            {
                text = "<color=#" + Color + ">" + text + "</color>";

            }

            return text;
        }

        /// <summary>
        /// clear screen log
        /// </summary>
        public void ClearScreen()
        {
            txtImputConsola.text = "";
            txtLogConsola.text = "";
            _LineaIngresada.Clear();

        }

        public void Reset()
        {
            SetValoresVisualizcionConsola();
        }

        private void SetValoresVisualizcionConsola()
        {

            _backGroundLogImg.color = new Color32(0, 47, 111, 150);
            txtLogConsola.color = new Color(255, 255, 255, 255);

            //CanvasConsola.SetActive(false);
            CanvasConsola.GetComponent<Canvas>().enabled = false;
            txtImputConsola.image.enabled = false;
            txtImputConsola.textComponent.color = new Color(255, 255, 255, 255);
            //LogBackGroundImage = null;


            _ShowSintaxisError = true; //te dice si el comando esta mal escrito o no
            _PauseGame = true;

            if (modo_Debug == true)
            {
                txtLogConsola.fontSize = 10;
                txtImputConsola.textComponent.fontSize = 10;
            }
            else
            {
                txtLogConsola.fontSize = 5;
                txtImputConsola.textComponent.fontSize = 5;
            }
        }

        void ChangeText(InputField input)
        {
            txtLogConsola.text = txtLogConsola.text + input.text + "\r\n";
        }

        public void SetText(string data)
        {
            txtLogConsola.text = txtLogConsola.text + data + "\r\n";
        }

        public void OpenCloseConsola(bool Abrir)
        {
            _ConsolaAbierta = Abrir;
            CanvasConsola.GetComponent<Canvas>().enabled = Abrir;
            txtImputConsola.enabled = Abrir;

            if (Abrir == true) //le doy foco al input
            {
                if (_PauseGame == true)
                    Time.timeScale = 0;

                txtImputConsola.text = "1234"; //esto corrige que se dibuje mal en pantalla al principio
                txtImputConsola.Select();
                txtImputConsola.ActivateInputField();
                txtImputConsola.text = "";
            }
            else
            {
                if (_PauseGame)
                    Time.timeScale = 1;
            }
        }

        public bool ConsolaAbierta
        {
            get
            {
                return _ConsolaAbierta;
            }
        }

        private void SepararComandoParametros(ref string Comando, ref string Parametros)
        {
            int nPos;

            Comando = Comando.Trim(); //le saco los espacios al final y al inicio del comando ingresado
            nPos = Comando.IndexOf(" "); //busco si tiene un espacio

            if (nPos > 0)
            {
                Parametros = Comando.Substring(nPos + 1); //obtengo los parámetros
                Comando = Comando.Substring(0, nPos); //obtengo solo el comando!
            }
            else
            {
                Parametros = "";
            }

        }

        private void MostrarComando(Boolean ComandoAnterior)
        {
            try
            {
                if (ComandoAnterior == true) //aprete para arriba
                {
                    _nIndiceComando--;
                }
                else
                {
                    _nIndiceComando++;
                }

                txtImputConsola.text = _ComandoIngresado[_nIndiceComando];
                txtImputConsola.Select(); //selecciono el input
                txtImputConsola.ActivateInputField(); //y le doy foco!

                //var pos = txtImputConsola.selectionFocusPosition;
                //txtImputConsola.selectionFocusPosition = pos;

                txtImputConsola.caretPosition = txtImputConsola.text.Length; //mando el cursor al final
            }
            catch
            {

            }
        }

        private void LeerComando()
        {

            _Comando = txtImputConsola.text;

            WriteLine(_Comando); //escribo la línea en la consola
            _ComandoIngresado.Add(_Comando); //lo agrego al buffer de comandos
            _nIndiceComando = _ComandoIngresado.Count;
            SepararComandoParametros(ref _Comando, ref _Parametros); //separo el comando de los párametros, luego, cada comando separará la cantidad de comandos necesarios
            Commands.commandInstance.ExecuteCommand(_Comando, _Parametros, ref _Resultado); //ejecuto el comando!!!!!!!!1!!1!uno!!1!
            if (_Resultado != "")
            {
                WriteLine(_Resultado);
            }
            txtImputConsola.text = ""; //limpio el input
            txtImputConsola.Select(); //selecciono el input
            txtImputConsola.ActivateInputField(); //y le doy foco!
        }

        void Update()
        {


            if (Input.GetKeyDown(ConsoleOpenKey)) 
            {

                if (ManagerConsola.instance.ConsolaAbierta == true)
                {
                    ManagerConsola.instance.OpenCloseConsola(false);
                }
                else
                {
                    ManagerConsola.instance.OpenCloseConsola(true);
                }

            }

            if (_ConsolaAbierta == true)
            {

                //busco si el primer caracter del imput es el que uso para abrir la consola
                quitarCaracterOpenConsola();

                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    LeerComando();
                }

                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    MostrarComando(true); 
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    MostrarComando(false);
                }

                if (Input.GetKeyDown(KeyCode.PageDown))
                {
                    DesplazarLog(false);
                }

                if (Input.GetKeyDown(KeyCode.PageUp))
                {
                    DesplazarLog(true);
                }
            }
        }


        private void quitarCaracterOpenConsola()
        {
            if (txtImputConsola.text.Length > 0)
            {
                char aux = (char)ConsoleOpenKey;

                if (txtImputConsola.text.Substring(0, 1) == aux.ToString())
                {
                    txtImputConsola.text = txtImputConsola.text.Substring(1);
                }
            }
        }

        private void DesplazarLog(bool up)
        {
            if (up)
            {
                if (_PosFinalHastaDondeMuestro > C_MAX_LINEAS)
                {
                    //obtengo el inicio de donde me voy a parar
                    _PosInicio = _PosFinalHastaDondeMuestro - C_MAX_LINEAS;
                    _PosInicio--; 
                    if (_PosInicio <0)
                    {
                        _PosInicio = 0;
                    }

                    //obbtengo el final de hasta donde voy a mostrar
                    _PosFinal = _PosInicio + C_MAX_LINEAS;
                    if (_PosFinal > _PosFinalHastaDondeMuestro -1)
                    {
                        _PosFinal = _PosFinalHastaDondeMuestro - 1;
                    }

                    txtLogConsola.text = "";
                    for (int I=_PosInicio;I<=_PosFinal;I++)
                    {
                        txtLogConsola.text = txtLogConsola.text + _LineaIngresada[I] + "\r\n";
                    }
                    _PosFinalHastaDondeMuestro--; //así puedo ir retrocediendo
                }

            }
            else
            {
                if (_PosFinalHastaDondeMuestro >= C_MAX_LINEAS)
                {
                    //obtengo el inicio de donde me voy a parar
                    _PosInicio = _PosFinalHastaDondeMuestro - C_MAX_LINEAS;
                    _PosInicio++;
                    if (_PosInicio > _LineaIngresada.Count-1)
                    {
                        _PosInicio = _LineaIngresada.Count - 1;
                    }

                    //obbtengo el final de hasta donde voy a mostrar
                    _PosFinal = _PosInicio + C_MAX_LINEAS;
                    if (_PosFinal > _PosFinalHastaDondeMuestro - 1)
                    {
                        _PosFinal = _PosFinalHastaDondeMuestro;
                    }

                    txtLogConsola.text = "";
                    for (int I = _PosInicio; I <= _PosFinal; I++)
                    {
                        txtLogConsola.text = txtLogConsola.text + _LineaIngresada[I] + "\r\n";
                    }
                    _PosFinalHastaDondeMuestro++; //así puedo ir retrocediendo

                    if (_PosFinalHastaDondeMuestro > _LineaIngresada.Count -1)
                    {
                        _PosFinalHastaDondeMuestro = _LineaIngresada.Count - 1;
                    }
                }
            }
        }

    }
}
