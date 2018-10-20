using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using InGameConsole;
using System;

public class ExampleConsole : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        //Commands.commandInstance.AddCommand
        CreateCommands();

	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    private void Write(string text)
    {
        ManagerConsola.instance.WriteLine(text);
    }

    private void CreateCommands()
    {
        Commands.commandInstance.AddCommand("/help", "Lista de comandos existentes", Help, "");
        Commands.commandInstance.AddCommand("/cls", "limpia la pantalla", LimpiarPantalla, "");
        Commands.commandInstance.AddCommand("/colorfondo", "Cambia el color de fondo en RGBA", CambiarColorFondo, "");
        Commands.commandInstance.AddCommand("/colorfuente", "Cambia el color de la fuente en RGBA", CambiarColorFuente, "");
        Commands.commandInstance.AddCommand("/sintaxis", "Activa y desactiva el mensaje de comando mal escrito", SintaxisOnOff, "");
        Commands.commandInstance.AddCommand("/crestart", "Reinicia la interfaz de la consola", RestartConsola, "");
        Commands.commandInstance.AddCommand("/test", "TEST TEST TEST", Test, "");
        Commands.commandInstance.AddCommand("/logsize","change font size of the log",ChangeLogFontSize);
        Commands.commandInstance.AddCommand("/addimg", "add a image to log background", AddImgLogBackground);
        Commands.commandInstance.AddCommand("/testlines", "add n lines in log screen", TestLines);
    }

    private void TestLines(string command, string parameters)
    {
        for (int I=0;I <=20;I++)
        {
            Write(I.ToString() + "  " + parameters);
        }
    }

    private void AddImgLogBackground(string command, string parameters)
    {
        
        ManagerConsola.instance.LogBackGroundImage = Resources.Load<Sprite>(parameters);
        Write(ManagerConsola.instance.LogBackGroundImage.ToString());
    }

    private void ChangeLogFontSize(string command, string parameters)
    {
        ManagerConsola.instance.LogFontSize = Convert.ToInt16(parameters);
    }

    private void Test(string comando, string parametros)
    {
        //txtLogConsola.text = txtLogConsola.text + "<color=#ff00ffff> We are <b>not</b></color> \r\n";

        Write("<b>" + parametros + " </b><i>" + parametros + " </i><color=#00ff00ff>" + parametros + "</color>");
        Write("<b>" + parametros + " </b><i>" + parametros + " </i><color=#ff00ffff>" + parametros + "</color>");
        Write("<b>" + parametros + " </b><i>" + parametros + " </i><color=#000080ff>" + parametros + "</color>");
        Write("<b>" + parametros + " </b><i>" + parametros + " </i><color=#ffa500ff>" + parametros + "</color>");
        Write("<b>" + parametros + " </b><i>" + parametros + " </i><color=#800080ff>" + parametros + "</color>");
        Write("<b>" + parametros + " </b><i>" + parametros + " </i><color=#ff0000ff>" + parametros + "</color>");
        Write("<b>" + parametros + " </b><i>" + parametros + " </i><color=#c0c0c0ff>" + parametros + "</color>");
        Write("<b>" + parametros + " </b><i>" + parametros + " </i><color=#008080ff>" + parametros + "</color>");
        Write("<b>" + parametros + " </b><i>" + parametros + " </i><color=#ffff00ff>" + parametros + "</color>");
        Write("<b>CONSOLE MANAGER WRITE TEST</b>");
        Write(ManagerConsola.instance.FormatWrite(parametros, "ffff00BE", true, true));
    }

    private void CambiarColorFuente(string comando, string parametros)
    {
        try
        {

            if (parametros == "restart")
            {
                ManagerConsola.instance.FontColor(new Color32(50, 50, 50, 255));
                Write("Color fondo por default");
                return;
            }

            char delimitador = ' ';
            string[] sAux = parametros.Split(delimitador);
            byte[] nColor = new byte[4];
            for (int I = 0; I < 4; I++)
            {
                nColor[I] = Convert.ToByte(sAux[I]);
            }
            ManagerConsola.instance.FontColor(new Color32(nColor[0], nColor[1], nColor[2], nColor[3]));

        }
        catch
        {
            Write("Error de sintaxis en los argumentos");
            Write("Formato RGBA");
            Write("Ejemplo /Ccolorfuente 0 47 111 150");
            Write("restart para color default");
            Write("Ejemplo /colorfuente restart");
        }
    }

    private void RestartConsola(string comando, string parametros)
    {
        ManagerConsola.instance.Reset();
    }

    private void SintaxisOnOff(string comando, string parametros)
    {
        if (parametros == "?")
        {
            Write(ManagerConsola.instance.ShowSintaxError.ToString());
            return;
        }
        else
        {
            if (ManagerConsola.instance.ShowSintaxError == true)
            {
                ManagerConsola.instance.ShowSintaxError = false;
            }
            else
            {
                ManagerConsola.instance.ShowSintaxError = true;
            }

            Write(ManagerConsola.instance.ShowSintaxError.ToString());
        }
    }

    private void CambiarColorFondo(string comando, string parametros)
    {
        try
        {

            if (parametros == "restart")
            {
                //imgFondo.color = new Color32(0, 47, 111, 150);
                ManagerConsola.instance.BackColor(new Color32(0, 47, 111, 150));
                Write("Color fondo por default");
                return;
            }

            char delimitador = ' ';
            string[] sAux = parametros.Split(delimitador);
            byte[] nColor = new byte[4];
            for (int I = 0; I < 4; I++)
            {
                nColor[I] = Convert.ToByte(sAux[I]);
            }
            ManagerConsola.instance.BackColor(new Color32(nColor[0], nColor[1], nColor[2], nColor[3]));

        }
        catch
        {
            Write("Error de sintaxis en los argumentos");
            Write("Formato RGBA");
            Write("Ejemplo /ColorFondo 0 47 111 150");
            Write("restart para color default");
            Write("Ejemplo /ColorFondo restart");
        }

    }

    private void LimpiarPantalla(string comando, string parametros)
    {
        ManagerConsola.instance.ClearScreen();
    }

    private void Help(string comando, string parametros)
    {
        Write("");

        string name = "";
        string description = "";
        string parameters = "";
        bool enabled = true;
        bool auxbool = true;

        for (int I = 0; I < Commands.commandInstance.CommandCount; I++)
        {
            Commands.commandInstance.showCommand(I, ref name, ref description, ref parameters, ref enabled, ref auxbool);
            Write("<color=#00ff00ff><b>" + name + "</b></color> " + description);
        }
        Write("");
    }


}
