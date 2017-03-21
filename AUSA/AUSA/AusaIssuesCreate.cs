﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Interactions;
using System.Linq;
using OpenQA.Selenium.Support.UI;
using System.Collections.Generic;


namespace AUSA  
{
    
    [TestClass]
    public class ausaIssuesCreate : ausaFieldsConfiguration
    {

        [TestInitialize]
        public void seTup()
        {
            driver = new ChromeDriver("C:\\Selenium");            
            driver.Manage().Window.Maximize();
        }       
        [TestMethod]
        public void ausaCreatePartes()
        {
            try
            {
                Actions action = new Actions(driver);
                if (driver.PageSource.Contains("No se puede acceder a este sitio web"))
                {                    
                    Console.WriteLine("ITS NO ESTA DISPONIBLE");
                    return;
                }
                
                driver.Navigate().GoToUrl(baseUrl);
                driver.FindElement(By.Id("BoxLogin")).SendKeys("00001");
                driver.FindElement(By.Id("BoxPassword")).SendKeys("00001");
                driver.FindElement(By.Id("BtnLogin")).Click();
                System.Threading.Thread.Sleep(3000);
                string lPartes = driver.FindElement(By.XPath("//div[7] / div / ul / li[5] / a")).Text;                
                IWebElement Partes = driver.FindElement(By.LinkText(lPartes));
                action.ClickAndHold(Partes).Perform();
                System.Threading.Thread.Sleep(1000);
                mPartes = driver.FindElement(By.XPath("// div[7] / div / ul / li[5] / ul / li / a")).Text;
                driver.FindElement(By.LinkText(mPartes)).Click();                
                System.Threading.Thread.Sleep(4000);
                string newHandle = driver.WindowHandles.Last(); //Para moverse a otro tab de ventana
                var newTab = driver.SwitchTo().Window(newHandle);
                var tabExpected = mPartes;
                Assert.AreEqual(tabExpected, newTab.Title);    // termina logica para enfocarse en otrotab            
                System.Threading.Thread.Sleep(3000);
                driver.FindElement(By.Id("ctl00_ContentZone_BtnCreate")).Click();
                System.Threading.Thread.Sleep(3000);
                selectDropDownClick("ctl00_ContentZone_cmb_template_cmb_dropdown");
                driver.FindElement(By.Id("ctl00_ContentZone_BtnConfirmTemplate")).Click();
                System.Threading.Thread.Sleep(4000);
                createPartes();
                System.Threading.Thread.Sleep(4000);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw;
            }
        }

        public void createPartes()
        {
            System.Threading.Thread.Sleep(1000);
            string newHandle = driver.WindowHandles.Last(); //Para moverse a otro tab de ventana
            var newTab = driver.SwitchTo().Window(newHandle);
            var tabExpected = "#Parte";
            Assert.AreEqual(tabExpected, newTab.Title);
            selectDropDownClick("ctl00_ContentZone_cmb_priority_cmb_dropdown");
            System.Threading.Thread.Sleep(4000);
            tipoSel = driver.FindElement(By.Id("ctl00_ContentZone_txt_type_box_data")).GetAttribute("value");
            //Filling out all data
            selectDropDownClick("ctl00_ContentZone_cmb_severity_cmb_dropdown");//Gravedad
                selectDropDownClick("ctl00_ContentZone_cmb_assigned_cmb_dropdown");//Asignado
            if (driver.FindElements(By.Id(supervisorT)).Count!= 0)
            {
                selectDropDownClick(supervisorT);//Supervisor            	
            }           
            System.Threading.Thread.Sleep(1000);
            selectDropDownClick(tValoresT);
            System.Threading.Thread.Sleep(1000);
            notEmptyDropDown(direcT);
            driver.FindElement(By.Id("ctl00_ContentZone_ctlPkm_txt_PkmKm_box_data")).SendKeys(ranNumbr(10, 900) + "");
            driver.FindElement(By.Id("ctl00_ContentZone_ctlPkm_txt_PkmM_box_data")).SendKeys(ranNumbr(1, 900) + "");
            System.Threading.Thread.Sleep(1000);
            notEmptyDropDown(ramalsT);
            driver.FindElement(By.Id(locationT)).SendKeys("Buenos Aires");
            driver.FindElement(By.Id(observaT)).SendKeys("QA issue created by Automation Script");
            System.Threading.Thread.Sleep(2000);
            datosSection();
            System.Threading.Thread.Sleep(1000);
            ranclickOption(dOption, 1, dOption.Length);
            System.Threading.Thread.Sleep(3000);
            if (driver.FindElement(By.Id(dOption[4])).Selected)
            {
                driver.FindElement(By.Id(vVolcado)).SendKeys(ranNumbr(1, 99) + "");
            }
            System.Threading.Thread.Sleep(2000);
            if (tipoSel.Equals("Incidente") || tipoSel.Equals("Accidente"))
            {
                ranclickOption(vOption, 1, vOption.Length);
                for (int i = 1; i < vOption.Length; i++)
                {
                    if (driver.FindElement(By.Id(vOption[i])).Selected)
                    {
                        System.Threading.Thread.Sleep(1000);
                        driver.FindElement(By.Id(vOptionT[i])).SendKeys(ranNumbr(1, 99) + ""); ;
                    }
                }
            }
            System.Threading.Thread.Sleep(500);
            if (driver.FindElements(By.Id(communicationField)).Count!=0)
            {
                communicationSection();
            }
            
            System.Threading.Thread.Sleep(1500);
            driver.FindElement(By.Id(issueCreateBtn)).Click();
            System.Threading.Thread.Sleep(2500);
        }

      
     
        public static void datosSection()
        {
            System.Threading.Thread.Sleep(1500);
            driver.FindElement(By.Id(datoBtn)).Click();
            System.Threading.Thread.Sleep(1000);
            if (tipoSel.Equals("Incidente") || tipoSel.Equals("Accidente"))
            {
                driver.FindElement(By.Id(typeAccidentes)).Click();
                System.Threading.Thread.Sleep(500);
                ranClick("ctl00_ContentZone_mc_typeOfAccident_ctl", 19, 23);
                System.Threading.Thread.Sleep(400);
                driver.FindElement(By.Id(typeImpacto)).Click();
                System.Threading.Thread.Sleep(500);
                ranClick("ctl00_ContentZone_mc_causal_ctl", 19, 23);
                System.Threading.Thread.Sleep(500);
            }
            driver.FindElement(By.Id("ctl00_ContentZone_txt_causes_box_data")).SendKeys("This was written by automation scrript for Test Purpose");
            driver.FindElement(By.Id("ctl00_ContentZone_txt_information_box_data")).SendKeys("This was written by automation scrript for Test Purpose");
            driver.FindElement(By.Id("ctl00_ContentZone_txt_observations_box_data")).SendKeys("This was written by automation scrript for Test Purpose");
            driver.FindElement(By.Id("ctl00_ContentZone_txt_note_box_data")).SendKeys("This was written by automation scrript for Test Purpose");
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.Id(cameraSel)).Click();
            System.Threading.Thread.Sleep(500);
            ranClick("ctl00_ContentZone_mcCameras_ctl", 105, 119);
            System.Threading.Thread.Sleep(400);
            driver.FindElement(By.Id(cameraSel)).Click();
        }
        public void communicationSection()
        {
            System.Threading.Thread.Sleep(1000);
            driver.FindElement(By.Id(communicationField)).Clear();
            driver.FindElement(By.Id(communicationField)).SendKeys("Communication"+" - "+ranNumbr(1,99)+" QA Automation" );
            System.Threading.Thread.Sleep(500);
            selectDropDownClick(newCommunication);
            System.Threading.Thread.Sleep(500);
            selectDropDownClick(medioField);
            System.Threading.Thread.Sleep(500);
            selectDropDownClick(motiveField);
            System.Threading.Thread.Sleep(500);
            selectDropDownClick(originDestination);
            System.Threading.Thread.Sleep(2000);
            notEmptyDropDown(originDest);
            System.Threading.Thread.Sleep(500);
            selectDropDownClick(importanceField);
            System.Threading.Thread.Sleep(500);
            driver.FindElement(By.Id(subjectField)).SendKeys("Created by Automation Script");
            driver.FindElement(By.Id(commentField)).SendKeys("This Communication was created by an automation script for testing purpose");
            System.Threading.Thread.Sleep(1000);
        }
        public static void grabarDatosFichero() throws Exception
        {
            beginDate = driver.findElement(By.id("ctl00_ContentZone_dt_opentime_box_date")).getAttribute("value");
            tempText1 = driver.findElement(By.id("ctl00_ContentZone_txt_template_box_data")).getAttribute("value");
            sevText = new Select(driver.findElement(By.id("ctl00_ContentZone_cmb_severity_cmb_dropdown"))).getFirstSelectedOption();
        sevText1 = sevText.getText();
			priorText = new Select(driver.findElement(By.id("ctl00_ContentZone_cmb_priority_cmb_dropdown"))).getFirstSelectedOption();
        priorText1 = priorText.getText();
			typeText = driver.findElement(By.id("ctl00_ContentZone_txt_type_box_data")).getAttribute("value");
        assignedText = new Select(driver.findElement(By.id(asignadoT))).getFirstSelectedOption();
        assignedText1 = assignedText.getText();
			locateText = driver.findElement(By.id("ctl00_ContentZone_txt_location_box_data")).getAttribute("value");
        autopistaText = new Select(driver.findElement(By.id(tValoresT))).getFirstSelectedOption();
        autopistaText1 = autopistaText.getText();
			bandaText = new Select(driver.findElement(By.id(direcT))).getFirstSelectedOption();
        bandaText1 = bandaText.getText();
			PkmText = driver.findElement(By.id("ctl00_ContentZone_ctlPkm_txt_PkmKm_box_data")).getAttribute("value");
        PkmText1 = driver.findElement(By.id("ctl00_ContentZone_ctlPkm_txt_PkmM_box_data")).getAttribute("value");
        ramalsText = new Select(driver.findElement(By.id(ramalsT))).getFirstSelectedOption();
        ramalsText1 = ramalsText.getText();
			observacionesText = driver.findElement(By.id("ctl00_ContentZone_txt_comments_box_data")).getAttribute("value");
			if (driver.findElements(By.id(supervisorT)).size()!=0){
				supervisorText = new Select(driver.findElement(By.id("ctl00_ContentZone_cmb_assigned_cmb_dropdown"))).getFirstSelectedOption();
        supervisorText1 = supervisorText.getText();
				supervT = true;
          }
    Thread.sleep(1000);	  					
			if (typeText.equals("Incidente") || typeText.equals("Accidente")){
				typeAcc = driver.findElement(By.id("ctl00_ContentZone_mc_typeOfAccident_txt_selected")).getAttribute("value");
    typeImpact = driver.findElement(By.id("ctl00_ContentZone_mc_causal_txt_selected")).getAttribute("value");

}
cAparente = driver.findElement(By.id("ctl00_ContentZone_txt_causes_box_data")).getAttribute("value");
						if (cAparente == null){
								cAparente = "";
						}
			infoComp = driver.findElement(By.id("ctl00_ContentZone_txt_information_box_data")).getAttribute("value");
					if (infoComp == null){
								infoComp = "";
					}
			obserGenerales = driver.findElement(By.id("ctl00_ContentZone_txt_observations_box_data")).getAttribute("value");
					if (obserGenerales == null){
							obserGenerales = "";
					}
			notaCentro = driver.findElement(By.id("ctl00_ContentZone_txt_note_box_data")).getAttribute("value");
					if (notaCentro == null){
						notaCentro = "";
					}
					mcCamerasS = driver.findElements(By.xpath("//*[contains(@id, 'ctl00_ContentZone_mcCameras_ctl')]"));
					cameraOpt  = new boolean[mcCamerasS.size()];
					cameraSelT = new String[mcCamerasS.size()];
					String[] del2 = new String[mcCamerasS.size()];
driver.findElement(By.id(cameraSel)).click();
		            	for (i = 0; i<= mcCamerasS.size()-1;i++){		            		
		            		del2[i] = mcCamerasS.get(i).getAttribute("id");
cameraOpt[i] = driver.findElement(By.xpath("//*[@id="+"'"+del2[i]+"'"+"]")).isSelected();
		            		if (cameraOpt[i]){
		            			camCount = camCount + 1;
		            			cameraSelT[i]=driver.findElement(By.xpath("//label[@for="+"'"+del2[i]+"'"+"]")).getText();
		            		}
		            	}
		            	Thread.sleep(1000);
		            	driver.findElement(By.id(cameraSel)).click();
			for (i = 1; i<dOption.length;i++){
					options[i] = driver.findElement(By.xpath("//label[@for="+"'"+dOption[i]+"'"+"]")).getText();
dOptionChecked[i] = driver.findElement(By.id(dOption[i])).isSelected();
					if (options[i].equals("Vehículos volcados")){	  									  								
						vVolcadosT = "Vehiculos volcados";
					}
				}
			
			if (typeText.equals("Incidente") || typeText.equals("Accidente")){
				for (int i = 1; i<vOption.length;i++){
					options1[i] = driver.findElement(By.xpath("//label[@for="+"'"+vOption[i]+"'"+"]")).getText();
vOptionTSel[i] = driver.findElement(By.id(vOption[i])).isSelected();					
					}
				comTitle = driver.findElement(By.id(communicationField)).getAttribute("value");
newCom = new Select(driver.findElement(By.id(newCommunication))).getFirstSelectedOption();
newComSel = newCom.getText();
				 if (newComSel.equals(null)){
					 newComSel = "";
				 }
				 comMean = new Select(driver.findElement(By.id(medioField))).getFirstSelectedOption();
comMeanSel = comMean.getText();
					 if (comMeanSel.equals(null)){
						 comMeanSel = "";
					 }
					 motiveD = new Select(driver.findElement(By.id(motiveField))).getFirstSelectedOption();
motiveSel = comMean.getText();
						 if (motiveSel.equals(null)){
							 motiveSel = "";
						 } 
				    originC = new Select(driver.findElement(By.id(originDestination))).getFirstSelectedOption();
originSel = originC.getText();
					 	if (originSel.equals(null)){
					 		originSel = "";
					 	}
					 if (originSel!=null){	
					 	originC_DestinaC = new Select(driver.findElement(By.id(originDest))).getFirstSelectedOption();
originC_DestSel = originC_DestinaC.getText();
					 		if (originC_DestSel.equals(null)){
					 			originC_DestSel = "";
					 		}
					 	}else{
					 		originC_DestSel = "";
					 	}
					 	importanceC = new Select(driver.findElement(By.id(importanceField))).getFirstSelectedOption();
importanceSel = importanceC.getText();
					 			if (importanceSel.equals(null)){
					 				importanceSel = "";
					 			}
					 	matterCom = driver.findElement(By.id(subjectField)).getAttribute("value");
commentCom = driver.findElement(By.id(commentField)).getAttribute("value");
					 	
					}
			
	}
		public static void crearFichero() throws Exception
{
			if (errorCreate){
        verFile = "crearPartesResultdosErrFile";
    } else{
        verFile = "crearPartesResultadosSuccess";
    }
    File oldFile = new File("C:\\Selenium\\"+verFile+"_OLD.txt");
			if (oldFile.exists()){
				oldFile.delete();
			}
			File result = new File("C:\\Selenium\\" + verFile + "_NEW.txt");
			if (result.exists()){
				result.renameTo(new File("C:\\Selenium\\"+verFile+"_OLD.txt"));
			}			
			FileOutputStream fis = new FileOutputStream(new File(result.toString()));
PrintStream out = new PrintStream(fis);
PrintStream old = System.out;
			System.setOut(out);
			if (parteNumber!=null){
				System.out.println("#Parte: "+parteNumber);
			}									
			System.out.println("Fecha Inicio: "+beginDate);
System.out.println("Plantilla: "+tempText1);
System.out.println("Gravedad: "+sevText1);
System.out.println("Prioridad: "+priorText1);
System.out.println("Tipo: "+typeText);
System.out.println("Asignado: "+assignedText1);
			if (supervT){
				System.out.println("Supervisor: "+supervisorText1);
			}	  					
			System.out.println("Autopista: "+autopistaText1);
System.out.println("Banda: "+bandaText1);
System.out.println("PKM(Km+m): "+PkmText+"+"+PkmText1);
System.out.println("Ramales: "+ramalsText1);
System.out.println("Localización: "+locateText);
System.out.println("Observaciones: "+observacionesText);
Thread.sleep(1000);	  					
			if (typeText.equals("Incidente") || typeText.equals("Accidente")){
				System.out.println("Tipo de Accidentes: "+ typeAcc);
System.out.println("Tipo de Impacto: "+typeImpact);
			}
			System.out.println("Causas Aparentes del Hecho: "+cAparente);
System.out.println("Información complementaria: "+infoComp);
System.out.println("Observaciones Generales: "+obserGenerales);
System.out.println("Nota del centro de operaciones: "+notaCentro);
    			if (camCount > 1){
    				System.out.print("Camara/s Seleccionada/s: ");
    			}else{
    				System.out.print("Camara Seleccionada: ");
    			}
			for (i = 0; i<= mcCamerasS.size()-1;i++){
				if (cameraOpt[i]){
						if (camCount > 1){
							System.out.print(cameraSelT[i]+"; ");
						}else{
							System.out.print(cameraSelT[i]);
						}
        			}
			}
			System.out.println("");
System.out.println("");
			for (i = 1; i<dOption.length;i++){
				if (dOptionChecked[i]){
					if (!options[i].equals("Vehículos volcados")){
						System.out.print("x"+options[i]+"    ");
					}
						if (options[i].equals("Vehículos volcados")){	  									  								
							System.out.print("xVehículos volcados"+ ": "+ volNumber);
						}
						}else{
							System.out.print(options[i]+"    ");
					}
				}
			System.out.println("");
			if (typeText.equals("Incidente") || typeText.equals("Accidente")){
				for (int i = 1; i<vOption.length;i++){
					if (vOptionTSel[i]){	  			  					
							System.out.print("x"+options1[i]+": "+vOptionNumber[i]+"    ");  			  							  			  							
							}else{
								System.out.print(options1[i]+"    ");
						}  			  				
				}
				System.out.println("");
System.out.println("");
System.out.println("Titulo de Comunicación: "+comTitle);
System.out.println("Tipo de Comunicación: "+newComSel);
System.out.println("Medio de Comunicación: "+comMeanSel);
System.out.println("Motivo de Comunicación: "+motiveSel);
System.out.println("Tipo Origen Destion: "+originSel);
System.out.println("Origen/Destino: "+originC_DestSel);
System.out.println("Importancia: "+importanceSel);
System.out.println("Asunto: "+matterCom);
System.out.println("Observaciones: "+commentCom);
					}
			
				fis.close();
				System.out.flush();
System.setOut(old);
				
			
	}

        private Boolean isElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }catch (NoSuchElementException e)
            {
                return false;
            }

        }
      
       

[TestCleanup]
        public void tearDown()
        {
            driver.Quit();
        }

    }
}