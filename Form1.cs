﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimitivasGráficas
{
    public partial class Form1 : Form
    {
        private Image imagePrincipal;
        private Bitmap imagePrincp;
        private Bitmap imageViewPort;
        private Ponto pontAtual;
        private PilhaRedo imagens;

        private Poligono poligAtual;
        private Ponto2 inicial, atual;
        private List<Poligono> listPoli;
        private int ultimoselecionado;
        private bool pintar;

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            imagePrincp = new Bitmap(1409, 537);
            imagePrincipal = imagePrincp;

            pontAtual = new Ponto();
            pontAtual.XInicio = -99999.0;
            pontAtual.XFinal = -1.0;
           
            cbOpcoesLinha.SelectedIndex = 0;
            cbFormaSelecionada.SelectedIndex = 2;
            cbOpcoesCirculos.SelectedIndex = 0;

            imagens = new PilhaRedo();
            poligAtual = null;
            inicial = null;
            atual = null;
            picBoxPrincp.Focus();
            resetaPontos();
            listPoli = new List<Poligono>();
            ultimoselecionado = 0;
            pintar = false;
            ckFlooddFill.Enabled = false;
            ckScanLine.Enabled = false;
            txX.Text = "" + 100;
            txY.Text = "" + 100;
        }

        private void BtLimparTela_Click(object sender, EventArgs e)
        {
            imagePrincp = new Bitmap(1409, 537);
            imagePrincipal = imagePrincp;
            picBoxPrincp.Image = imagePrincp;
            pontAtual.resetaPontos();
            imagens.inicializar();

            inicializarPolig();
            picBoxPrincp.Focus();
            resetaPontos();
            listPoli = new List<Poligono>();
            cbPoligonos.Items.Clear();
            ultimoselecionado = 0;
            atualizaComboBox();
            pintar = false;
            ckFlooddFill.Enabled = false;
            ckScanLine.Enabled = false;
            txX.Text = "" + 100;
            txY.Text = "" + 100;

            imageViewPort = new Bitmap(100, 100);
            picBoxViewPort.Image = imageViewPort;
        }

        public void inicializarPolig()
        {
            poligAtual = null;
            atual = null;
            inicial = null;
        }

        private void chamaCriaFormas2()
        {
            string metodo = "";
            double Raio = 0;

            if (cbFormaSelecionada.SelectedIndex == 0)
            {
                Raio = verificaLimite(pontAtual);

                if(Raio != -999999)
                {
                    if (cbOpcoesCirculos.SelectedIndex == 0)
                    {
                        MetodosCriadoresdeCirculos.equacaoGeral(imagePrincp, pontAtual, Raio);
                        picBoxPrincp.Image = imagePrincp;
                        metodo = "EQUACAO";
                    }
                    else if (cbOpcoesCirculos.SelectedIndex == 1)
                    {
                        MetodosCriadoresdeCirculos.trignometria(imagePrincp, pontAtual, Raio);
                        picBoxPrincp.Image = imagePrincp;
                        metodo = "TRIGO";
                    }
                    else
                    {
                        MetodosCriadoresdeCirculos.pontoMedio(imagePrincp, pontAtual, Raio);
                        picBoxPrincp.Image = imagePrincp;
                        metodo = "PONTO";
                    }
                }

            }
            else if (cbFormaSelecionada.SelectedIndex == 1)
            {

                MetodosCriadoresdeCirculos.MidpointElipse(pontAtual, imagePrincp);
                picBoxPrincp.Image = imagePrincp;
                metodo = "ELIPSE";
            }
            else if(cbFormaSelecionada.SelectedIndex == 2)
            { 

                if (cbOpcoesLinha.SelectedIndex == 0)
                {
                    MetodosCriacaodeLinhas.DDA(imagePrincp, pontAtual);
                    picBoxPrincp.Image = imagePrincp;
                    metodo = "DDA";
                }
                else if (cbOpcoesLinha.SelectedIndex == 1)
                {
                    MetodosCriacaodeLinhas.EquacaoRealdaReta(imagePrincp, pontAtual);
                    picBoxPrincp.Image = imagePrincp;
                    metodo = "EQUACAOREAL";
                }
                else
                {
                    MetodosCriacaodeLinhas.bresenham2(imagePrincp, (int)pontAtual.XInicio, (int)pontAtual.XFinal, (int)pontAtual.YInicio, (int)pontAtual.YFinal);
                    picBoxPrincp.Image = imagePrincp;
                    metodo = "BRESENHAM";
                }
            }
            else
            {
                MetodosCriacaodeLinhas.DDA(imagePrincp, pontAtual);
                picBoxPrincp.Image = imagePrincp;
            }

            if(Raio != -999999 && cbFormaSelecionada.SelectedIndex != 3)
            {
                Ponto aux = new Ponto();
                aux.transefePontos(pontAtual);
                imagens.inserir(aux, metodo);
                pontAtual.resetaPontos();
            }
            
        }

        private void chamaCriaFormas(string opcao, Ponto novo)
        {
            double Raio;

            Raio = verificaLimite(novo);

            switch (opcao){

                case "EQUACAO":
                    MetodosCriadoresdeCirculos.equacaoGeral(imagePrincp, novo, Raio);
                    picBoxPrincp.Image = imagePrincp;
                    break;
                case "TRIGO":
                    MetodosCriadoresdeCirculos.trignometria(imagePrincp, novo, Raio);
                    picBoxPrincp.Image = imagePrincp;
                    break;
                case "PONTO":
                    MetodosCriadoresdeCirculos.pontoMedio(imagePrincp, novo, Raio);
                    picBoxPrincp.Image = imagePrincp;
                    break;
                case "ELIPSE":
                    MetodosCriadoresdeCirculos.MidpointElipse(novo, imagePrincp);
                    picBoxPrincp.Image = imagePrincp;
                    break;
                case "DDA":
                    MetodosCriacaodeLinhas.DDA(imagePrincp, novo);
                    picBoxPrincp.Image = imagePrincp;
                    break;
                case "EQUACAOREAL":
                    MetodosCriacaodeLinhas.EquacaoRealdaReta(imagePrincp, novo);
                    picBoxPrincp.Image = imagePrincp;
                    break;
                case "BRESENHAM":
                    MetodosCriacaodeLinhas.bresenham2(imagePrincp, (int)novo.XInicio, (int)novo.XFinal, (int)novo.YInicio, (int)novo.YFinal);
                    picBoxPrincp.Image = imagePrincp;
                    break;
                case "VIEWPORT":
                    MetodosCriacaodeLinhas.DDA(imageViewPort, novo);
                    picBoxViewPort.Image = imageViewPort;
                    break;
                case "EQUACAO2":
                    MetodosCriadoresdeCirculos.equacaoGeral(imageViewPort, novo, Raio);
                    picBoxViewPort.Image = imageViewPort;
                    break;
                case "TRIGO2":
                    MetodosCriadoresdeCirculos.trignometria(imageViewPort, novo, Raio);
                    picBoxViewPort.Image = imageViewPort;
                    break;
                case "PONTO2":
                    MetodosCriadoresdeCirculos.pontoMedio(imageViewPort, novo, Raio);
                    picBoxViewPort.Image = imageViewPort;
                    break;
                case "ELIPSE2":
                    MetodosCriadoresdeCirculos.MidpointElipse(novo, imageViewPort);
                    picBoxViewPort.Image = imageViewPort;
                    break;
                case "DDA2":
                    MetodosCriacaodeLinhas.DDA(imageViewPort, novo);
                    picBoxViewPort.Image = imageViewPort;
                    break;
                case "EQUACAOREAL2":
                    MetodosCriacaodeLinhas.EquacaoRealdaReta(imageViewPort, novo);
                    picBoxViewPort.Image = imageViewPort;
                    break;
                case "BRESENHAM2":
                    MetodosCriacaodeLinhas.bresenham2(imageViewPort, (int)novo.XInicio, (int)novo.XFinal, (int)novo.YInicio, (int)novo.YFinal);
                    picBoxViewPort.Image = imageViewPort;
                    break;
            }          
                      
        }

        private void recriaPoligono(Poligono aux)
        {
            
            for (int i = 0; i < aux.PontosAtuais.Count; i++)
            {
                if(i >= 1)
                {                   
                    pontAtual.transferePonto2(aux.PontosAtuais[i-1], aux.PontosAtuais[i]);

                    chamaCriaFormas("DDA", pontAtual);
                }
            }

            pontAtual.transferePonto2(aux.PontosAtuais[aux.PontosAtuais.Count-1], aux.PontosAtuais[0]);

            chamaCriaFormas("DDA", pontAtual);
        }

        private void recriaPoligonoViewPort(Poligono aux)
        {

            for (int i = 0; i < aux.PontosViewPort.Count; i++)
            {
                if (i >= 1)
                {
                    pontAtual.transferePonto2(aux.PontosViewPort[i - 1], aux.PontosViewPort[i]);

                    chamaCriaFormas("VIEWPORT", pontAtual);
                }
            }

            pontAtual.transferePonto2(aux.PontosViewPort[aux.PontosViewPort.Count - 1], aux.PontosViewPort[0]);

            chamaCriaFormas("VIEWPORT", pontAtual);
        }

        private void recriaFormas()
        {
            imagePrincp = new Bitmap(1409, 537);
            imagePrincipal = imagePrincp;
            picBoxPrincp.Image = imagePrincp;
            pontAtual.resetaPontos();

            NoRedo aux = imagens.Inicio;
            List<NoRedo> retirados = new List<NoRedo>();

            while(aux != null)
            {
                if (aux.Poligon != null)
                    recriaPoligono(aux.Poligon);
                else
                    chamaCriaFormas(aux.Metodo, aux.Info);
                retirados.Add(aux);
                aux = aux.Prox;
            }

            imagens.inicializar();

           for(int i = retirados.Count-1; i >= 0; i--) { 

                if(retirados[i].Poligon != null)
                    imagens.inserir(retirados[i].Poligon, retirados[i].Metodo);
                else
                     imagens.inserir(retirados[i].Info, retirados[i].Metodo);
           }
          
        }

        private void recriaFormas2()
        {       
          
            NoRedo aux = imagens.Inicio;
            

            while (aux != null)
            {
                if (aux.Poligon != null)
                    recriaPoligonoViewPort(aux.Poligon);
                else
                {
                    Ponto pontAux = new Ponto();
                    pontAux.XInicio = aux.Info.XInicioWP;
                    pontAux.XFinal = aux.Info.XFinalWP;
                    pontAux.YInicio = aux.Info.YInicioWP;
                    pontAux.YFinal = aux.Info.YFinalWP;

                    chamaCriaFormas(aux.Metodo + "2", pontAux);
                }
                 
               
                aux = aux.Prox;
            }
            

        }

        public void atualizaComboBox()
        {
            try
            {
                dtPontosPoligono.DataSource = null;
                dtPontosPoligono.Rows.Clear();
                dtPontosPoligono.Refresh();
            }
            catch (Exception e) { }
           

            listPoli = new List<Poligono>();
            NoRedo aux = imagens.Inicio;
            
            while(aux != null)
            {
                if (aux.Poligon != null)
                    listPoli.Add(aux.Poligon);

                aux = aux.Prox;
            }

            List<string> posicoes = new List<string>();

            for (int i = 0; i < listPoli.Count; i++)
            {
                posicoes.Add("Poligono: "+(i+1));

            }

            
            cbPoligonos.Items.Clear();
            cbPoligonos.Items.AddRange(posicoes.ToArray());
            if(listPoli.Count != 0)
            {
                if (ultimoselecionado >= listPoli.Count)
                    ultimoselecionado--;

                cbPoligonos.SelectedIndex = ultimoselecionado;
                cbPoligonos_SelectedIndexChanged(null, null);
            }
            
           
            
        }

        private double verificaLimite(Ponto pontos)
        {
            double Raio = Math.Sqrt(Math.Pow((pontos.XFinal - pontos.XInicio), 2.0) + Math.Pow((pontos.YFinal - pontos.YInicio), 2.0));
             return Raio;                       
        }

        private void criaPoligono(MouseEventArgs e)
        {
            int pos;

            if(inicial == null)
            {
                inicial = new Ponto2(e.X, e.Y);
                poligAtual.addPonto(inicial);
                poligAtual.addPontoAtual(new Ponto2(e.X, e.Y));
            }
            else
            {
                if((e.X < poligAtual.getPontoOriginalX(0) - 5  || e.X > poligAtual.getPontoOriginalX(0) + 5) ||
                    (e.Y < poligAtual.getPontoOriginalY(0) - 5 || e.Y > poligAtual.getPontoOriginalY(0) + 5))
                {
                    atual = new Ponto2(e.X, e.Y);
                    poligAtual.addPonto(atual);
                    poligAtual.addPontoAtual(new Ponto2(e.X, e.Y));

                    pos = poligAtual.PontosOriginais.IndexOf(atual) - 1;
                    pontAtual.transferePonto2(poligAtual.PontosOriginais[pos], atual);

                    chamaCriaFormas2();
                    atual = null;
                }
                else
                {
                                                        
                    pontAtual.transferePonto2(poligAtual.PontosOriginais[poligAtual.PontosOriginais.Count-1]
                        , poligAtual.PontosOriginais[0]);

                    chamaCriaFormas2();                    
                    pontAtual.resetaPontos();
                    atualizaComboBox();
                    inicializarPolig();
                }
            }
        }

        private void PicBoxPrincp_MouseClick(object sender, MouseEventArgs e)
        {

            if (!pintar)
            {
                if (cbFormaSelecionada.SelectedIndex == 3)
                {
                    if (poligAtual == null)
                    {
                        poligAtual = new Poligono();

                        imagens.inserir(poligAtual, "POLIGONO");
                    }

                    criaPoligono(e);

                }
                else
                {
                    if (pontAtual.XInicio == -99999.0)
                    {
                        pontAtual.XInicio = e.X;
                        pontAtual.YInicio = e.Y;

                    }
                    else if (pontAtual.XFinal == -1.0)
                    {
                        pontAtual.XFinal = e.X;
                        pontAtual.YFinal = e.Y;

                        chamaCriaFormas2();
                    }

                    picBoxPrincp.Focus();
                }
            }
            else
            {
                if (listPoli.Count > 0)
                {
                    if (ckFlooddFill.Checked)
                    {
                        listPoli[(listPoli.Count - 1) - cbPoligonos.SelectedIndex].floodFill((int)e.X, (int)e.Y, imagePrincp);
                        picBoxPrincp.Image = imagePrincp;
                       
                    }
                    else if(ckScanLine.Checked)
                    {
                        listPoli[(listPoli.Count - 1) - cbPoligonos.SelectedIndex].carregaListaET(imagePrincp);
                        picBoxPrincp.Image = imagePrincp;
                    }

                }
            }
           
          
        }

        private void Button2_Click(object sender, EventArgs e)
       {
            imagens.retirar();

            if (!imagens.isEmpty())
            {
                 recriaFormas();
                
            }
            else
            {
                BtLimparTela_Click(null, null);
            }

            atualizaComboBox();
            picBoxPrincp.Focus();
        }

      

        private void resetaPontos()
        {
            rbOrigem.Checked = true;          
            txXescala.Text = "" + 0.0;
            txXtrans.Text = "" + 0.0;
            txAngulo.Text= "" + 0;            
            txYescala.Text = "" + 0.0;
            txYtrans.Text = "" + 0.0;
            txShearX.Text = "" + 0.0;
            txShearY.Text = "" + 0.0;
            txReflexaoX.Text = "" + 0.0;
            txReflexaoY.Text = "" + 0.0;
        }

        private void aplicaTransformacoes(Poligono poli)
        {
            Ponto2 pontRotacao = new Ponto2();
            double transX, transY;
            double escalaX, escalaY;
            double angulo;
            double cisaX, cisaY;
            double refleX, refleY;
            pontRotacao.X = 0;
            pontRotacao.Y = 0;


            if (rbCentro.Checked)
            {
                pontRotacao.X = poli.retornaCX();
                pontRotacao.Y = poli.retornaCY();

            }

            try
            {
                transX = double.Parse(txXtrans.Text);
                transY = double.Parse(txYtrans.Text);
            }
            catch (Exception e) { transX = 0.0; transY = 0.0; }

            try
            {
                escalaX = double.Parse(txXescala.Text);
                escalaY = double.Parse(txYescala.Text);
            }
            catch (Exception e) { escalaX = 1; escalaY = 1; }

            try
            {
                angulo = double.Parse(txAngulo.Text);
            }
            catch (Exception e) { angulo = 0.0; }

            try
            {
                cisaX = double.Parse(txShearX.Text);
            }
            catch (Exception e) { cisaX = 0.0; };

            try
            {
                cisaY = double.Parse(txShearY.Text);
            }
            catch (Exception e) { cisaY = 0.0; };

            try
            {
                refleX = double.Parse(txReflexaoX.Text);
            }
            catch (Exception e) { refleX = 0; };

            try
            {
                refleY = double.Parse(txReflexaoY.Text);
            }
            catch (Exception e) { refleY = 0; };

            poli.inicializarMatriz();

            if (refleX != 0)
                poli.reflexaoX(refleX);

            if (refleY != 0)
                poli.reflexaoY(refleY);

            if(cisaX != 0 || cisaY != 0)            
                poli.shearXY(cisaX, cisaY);            

            

            if (angulo != 0)
            {
                if (pontRotacao.X == 0 && pontRotacao.Y == 0)
                    poli.rotacao(angulo);
                else
                    poli.rotacao(angulo, pontRotacao.X, pontRotacao.Y);
            }
            
            
            if(escalaX != 0 || escalaY != 0)
            {
                escalaY = escalaY == 0 ? 1 : escalaY;
                escalaX = escalaX == 0 ? 1 : escalaX;
                if (pontRotacao.X == 0 && pontRotacao.Y == 0)
                    poli.escala(escalaX, escalaY);
                else
                    poli.escala(escalaX, escalaY, pontRotacao.X, pontRotacao.Y);
            }
           
            if(transX != 0 || transY != 0)
                 poli.translacao(transX, transY);

            poli.aplicarMA();            

        }

        private void BtAplicarTrans_Click(object sender, EventArgs e)
        {
            
            if(listPoli != null && listPoli.Count != 0)
            {
                aplicaTransformacoes(listPoli[(listPoli.Count-1) - cbPoligonos.SelectedIndex]);
                recriaFormas();
                atualizaComboBox();
            }
           
            
        }

        private void cbPoligonos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ultimoselecionado = cbPoligonos.SelectedIndex;
            dtPontosPoligono.DataSource = listPoli[(listPoli.Count - 1) - cbPoligonos.SelectedIndex].PontosAtuais;
            foreach (DataGridViewColumn column in dtPontosPoligono.Columns)
            {
                column.Width = 50;
            }
            dtPontosPoligono.Focus();
           
        }

        private void btPintar_Click(object sender, EventArgs e)
        {
            pintar = !pintar;
            ckFlooddFill.Enabled = pintar;
            ckScanLine.Enabled = pintar;
        }

        private void btViewport_Click(object sender, EventArgs e)
        {
            int widthViewBox, heightViewBox;
            try
            {             
                int.TryParse(txX.Text, out widthViewBox);
                int.TryParse(txY.Text, out heightViewBox);
            }
            catch(Exception ex)
            {
                widthViewBox = imagePrincp.Width;
                heightViewBox = imagePrincp.Height;
            }

            imageViewPort = new Bitmap(widthViewBox,heightViewBox);
            picBoxViewPort.Width = widthViewBox;
            picBoxViewPort.Height = heightViewBox;

            for(int i=0;i<listPoli.Count();i++)
            {
                listPoli[i].viewPort(picBoxPrincp.Width,picBoxPrincp.Height,imageViewPort);
                
            }

            NoRedo aux = imagens.Inicio;
            while(aux != null)
            {
                if(aux.Poligon == null)
                {
                    aux.Info.viewPort(picBoxPrincp.Width, picBoxPrincp.Height, imageViewPort);
                }

                aux = aux.Prox;

            }

            recriaFormas2();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void BtResetarCamp_Click(object sender, EventArgs e)
        {
            resetaPontos();
        }
       
    }
}
