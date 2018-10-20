using System;
using System.Collections.Generic;
using UnityEngine;

namespace InGameConsole
{
    public delegate void ExecCommand(string command, string parameters);

    internal class Commands : MonoBehaviour
    {
        internal const int C_NO_ERROR = 0;
        internal const int C_ERROR = 1;

        public string UnknowCommandMenssage;

        internal static Commands commandInstance = null;

        private List<ExecCommand>    _ExecCommand = new List<ExecCommand>();
        private List<string>         _name = new List<string>();
        private List<string>         _description = new List<string>();
        private List<string>         _parameters = new List<string>();
        private List<bool>           _EnabledCommand = new List<bool>();
        private List<bool>           _auxbool = new List<bool>();

        /// <summary>
        /// Number of existing commands
        /// </summary>
        internal int CommandCount
        {
            get
            {
                return _name.Count;
            }
        }

        void Awake()
        {

            if (commandInstance == null)
            {
                commandInstance = this;
            }
            else
            {
                Destroy(this);
            }

        }

        void Start()
        {
            //si no lo uso borrar!!!    
        }

        /// <summary>
        ///Add a command to be executed when necessary
        /// </summary>
        /// <param name="name">>Name of the command</param>
        /// <param name="description">Command description</param>
        /// <param name="command">Procedure where the command is executed</param>
        /// <param name="parameters">(Description of the parameters), default is empty, this is used at the time of execution</param>
        /// <param name="enabled">If it is true, the command can be executed, otherwise it will not</param>
        /// <param name="auxbool">Auxiliary parameter</param>
        internal void AddCommand(string name, string description, ExecCommand command, string parameters="",bool enabled = true, bool auxbool = true)
        {
            _ExecCommand.Add(command);
            _name.Add(name);
            _description.Add(description);
            _parameters.Add(parameters);
            _EnabledCommand.Add(enabled);
            _auxbool.Add(auxbool);
        }

        /// <summary>
        /// Delete a command
        /// </summary>
        /// <param name="name">Name of the command to be deleted</param>
        /// <returns>Returns 0 if it could be deleted, otherwise returns 1</returns>
        internal int DeleteCommand(string name)
        {
            for (int index = 0; index < _ExecCommand.Count; index++)
            {
                if (_name[index] == name)
                {
                    _ExecCommand.RemoveAt(index);
                    _name.RemoveAt(index);
                    _description.RemoveAt(index); 
                    _parameters.RemoveAt(index);
                    _EnabledCommand.RemoveAt(index);
                    _auxbool.RemoveAt(index);
                    return C_NO_ERROR;
                }
            }

            return C_ERROR;
        }

        /// <summary>
        /// Execute a command
        /// </summary>
        /// <param name="name">Name of the command to execute</param>
        /// <param name="parameters">Parametros of the command, each parameter must be separated by space, example: param1 param2 ... paramN</param>
        /// <param name="Result">Shows if there was an error</param>
        internal void ExecuteCommand(string name, string parameters, ref string Result)
        {

            Result = "";

            try
            {
                for (int index = 0; index < _ExecCommand.Count; index++)
                {
                    if (_name[index] == name)
                    {
                        _ExecCommand[index](name, parameters);
                        return;
                    }
                }
                Result = UnknowCommandMenssage;
            }
            catch (Exception err)
            {
                Result = err.Message;
            }
        }
        
        internal void showCommand(string name,ref string description, ref string parameters,ref bool enabled, ref bool auxbool)
        {
            for (int index = 0; index < _ExecCommand.Count;index++)
            {
                if (_name[index] == name)
                {
                    string aux = "";
                    showCommand(index, ref aux, ref description, ref parameters, ref enabled, ref auxbool);
                }
            }
        }

        internal void showCommand(int Index, ref string name, ref string description, ref string parameters,ref bool enabled, ref bool auxbool)
        {
            name = _name[Index];
            description = _description[Index];
            parameters = _parameters[Index];
            enabled = _EnabledCommand[Index];
            auxbool = _auxbool[Index];
        }
    }
}
