using System;
using System.Collections.Generic;
using System.Web.Services;

namespace CalculadoraWebService
{
    [WebService(Namespace = "http://miwebservice.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class Calculadora : WebService
    {
        [WebMethod]
        public int EvaluarInfija(string expresion)
        {
            Stack<int> valores = new Stack<int>();
            Stack<char> operadores = new Stack<char>();

            for (int i = 0; i < expresion.Length; i++)
            {
                if (expresion[i] == ' ')
                    continue;

                if (Char.IsDigit(expresion[i]))
                {
                    int valor = 0;
                    while (i < expresion.Length && Char.IsDigit(expresion[i]))
                    {
                        valor = (valor * 10) + (expresion[i] - '0');
                        i++;
                    }
                    valores.Push(valor);
                    i--;
                }
                else if (expresion[i] == '(')
                {
                    operadores.Push(expresion[i]);
                }
                else if (expresion[i] == ')')
                {
                    while (operadores.Peek() != '(')
                    {
                        valores.Push(AplicarOperador(operadores.Pop(), valores.Pop(), valores.Pop()));
                    }
                    operadores.Pop();
                }
                else if (EsOperador(expresion[i]))
                {
                    while (operadores.Count > 0 && Prioridad(operadores.Peek()) >= Prioridad(expresion[i]))
                    {
                        valores.Push(AplicarOperador(operadores.Pop(), valores.Pop(), valores.Pop()));
                    }
                    operadores.Push(expresion[i]);
                }
            }

            while (operadores.Count > 0)
            {
                valores.Push(AplicarOperador(operadores.Pop(), valores.Pop(), valores.Pop()));
            }

            return valores.Pop();
        }

        [WebMethod]
        public int EvaluarPrefija(string expresion)
        {
            Stack<int> stack = new Stack<int>();

            for (int i = expresion.Length - 1; i >= 0; i--)
            {
                if (Char.IsWhiteSpace(expresion[i]))
                    continue;

                if (Char.IsDigit(expresion[i]))
                {
                    int valor = 0;
                    int baseNumerica = 1;
                    while (i >= 0 && Char.IsDigit(expresion[i]))
                    {
                        valor += (expresion[i] - '0') * baseNumerica;
                        baseNumerica *= 10;
                        i--;
                    }
                    stack.Push(valor);
                }
                else if (EsOperador(expresion[i]))
                {
                    int op1 = stack.Pop();
                    int op2 = stack.Pop();
                    int resultado = AplicarOperador(expresion[i], op1, op2);
                    stack.Push(resultado);
                }
            }

            return stack.Pop();
        }

        private int AplicarOperador(char operador, int b, int a)
        {
            switch (operador)
            {
                case '+': return a + b;
                case '-': return a - b;
                case '*': return a * b;
                case '/': return a / b;
                default: throw new ArgumentException("Operador no válido");
            }
        }

        private bool EsOperador(char c)
        {
            return (c == '+' || c == '-' || c == '*' || c == '/');
        }

        private int Prioridad(char operador)
        {
            switch (operador)
            {
                case '+':
                case '-':
                    return 1;
                case '*':
                case '/':
                    return 2;
                default:
                    return 0;
            }
        }
    }
}
