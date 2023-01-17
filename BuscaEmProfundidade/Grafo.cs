using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuscaEmProfundidade
{
    class Vertice
    {
        public string Nome { get; set; }
        public int TempoDescoberta { get; set; }
        public int TempoTermino { get; set; }
        public string VerticePai { get; set; }
        public List<Vertice> ListaAdjacencia { get; set; }

        public Vertice(string nome)
        {
            Nome = nome;
            ListaAdjacencia = new List<Vertice>();
        }
    }

    class Grafo
    {
        public Dictionary<string, Vertice> Vertices;
        public List<string> OrdemVisitados, ArestasArvore;

        public Grafo()
        {
            Vertices = new Dictionary<string, Vertice>();
            OrdemVisitados = new List<string>();
            ArestasArvore = new List<string>();
        }

        public void AddArestas(string verticeOrigem, string verticeDestino)
        {
            if (!Vertices.ContainsKey(verticeOrigem))
                Vertices.Add(verticeOrigem, new Vertice(verticeOrigem));
            if (!Vertices.ContainsKey(verticeDestino))
                Vertices.Add(verticeDestino, new Vertice(verticeDestino));

            Vertice vertice1 = Vertices[verticeOrigem], vertice2 = Vertices[verticeDestino];

            vertice1.ListaAdjacencia.Add(vertice2);
            vertice2.ListaAdjacencia.Add(vertice1);
        }

        public void BuscaEmProfundidade(string verticeInicio, StreamWriter sw)
        {
            int tempo = 0;
            Vertice verticeIniciado = Vertices[verticeInicio];

            tempo = PercorrerVertices(verticeIniciado, tempo, sw);

            foreach (Vertice vertice in Vertices.Values)
            {
                if (vertice.TempoDescoberta == 0 && !vertice.Nome.Equals(verticeInicio))
                {
                    tempo = PercorrerVertices(vertice, tempo, sw);
                }
            }
        }

        public int PercorrerVertices(Vertice verticeExplorado, int tempo, StreamWriter sw)
        {
            MostrarListaVertice(verticeExplorado, sw);
            OrdemVisitados.Add(verticeExplorado.Nome);

            verticeExplorado.TempoDescoberta = ++tempo;

            foreach (Vertice verticeAdjacente in verticeExplorado.ListaAdjacencia)
            {
                if (verticeAdjacente.TempoDescoberta == 0)
                {
                    ArestasArvore.Add("{"+$"{verticeExplorado.Nome},{verticeAdjacente.Nome}"+"}");
                    verticeAdjacente.VerticePai = verticeExplorado.Nome;
                    tempo = PercorrerVertices(verticeAdjacente, tempo, sw);
                }
            }

            verticeExplorado.TempoTermino = ++tempo;

            return tempo;
        }

        public void MostrarVertices()
        {
            Console.WriteLine("Vertices: ");
            foreach (KeyValuePair<string, Vertice> vertice in Vertices)
            {
                Console.Write($"{vertice.Key} ");
            }
            Console.WriteLine($"\n\nTotal de Vertices: {Vertices.Count}");
        }

        public void MostrarListaVertice(Vertice vertice, StreamWriter sw)
        {
            Console.Write($"Y({vertice.Nome}) = " + "{ ");
            sw.Write($"Y({vertice.Nome}) = " + "{ ");

            foreach (Vertice verticeAux in vertice.ListaAdjacencia)
            {
                if (vertice.ListaAdjacencia.Last().Nome.Equals(verticeAux.Nome))
                {
                    Console.Write($"{verticeAux.Nome}");
                    sw.Write($"{verticeAux.Nome}");
                    break;
                }
                Console.Write($"{verticeAux.Nome}, ");
                sw.Write($"{verticeAux.Nome}, ");
            }

            Console.Write(" }\n");
            sw.Write(" }\n");
        }

        public void MostrarOrdemVisitados(StreamWriter sw)
        {
            Console.WriteLine("\nLista de visitados: ");
            sw.WriteLine("\nLista de visitados: ");
            foreach (string vertice in OrdemVisitados)
            {
                Console.Write($"{vertice} ");
                sw.Write($"{vertice} ");
            }
        }

        public void MostrarTabelaTempo(StreamWriter sw)
        {
            Console.WriteLine("\n\n|Vertice|TD\t|TT\t|Pai");
            sw.WriteLine("\n\n|Vertice|TD\t|TT\t|Pai");
            foreach (KeyValuePair<string, Vertice> vertice in Vertices)
            {
                string Nome = vertice.Value.Nome;
                int TempoDescoberta = vertice.Value.TempoDescoberta;
                int TempoTermino = vertice.Value.TempoTermino;
                string VerticePai = vertice.Value.VerticePai;

                Console.WriteLine($"|   {Nome}\t|{TempoDescoberta}\t|{TempoTermino}\t|{VerticePai}");
                sw.WriteLine($"|   {Nome}\t|{TempoDescoberta}\t|{TempoTermino}\t|{VerticePai}");
            }
        }

        public void MostrarArvore(StreamWriter sw)
        {
            Console.Write("\nE = {");
            sw.Write("\nE = {");
            foreach (string aresta in ArestasArvore)
            {
                Console.Write($" {aresta} ");
                sw.Write($" {aresta} ");
            }
            Console.Write("}");
            sw.Write("}");
        }
    }
}
