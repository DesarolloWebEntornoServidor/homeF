CREATE DATABASE  IF NOT EXISTS `homefinance` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `homefinance`;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: homefinance
-- ------------------------------------------------------
-- Server version	5.7.20-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `cajageneral`
--

DROP TABLE IF EXISTS `cajageneral`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `cajageneral` (
  `idCaja` int(11) NOT NULL AUTO_INCREMENT,
  `fechaSaldo` datetime NOT NULL,
  `saldo` decimal(8,2) unsigned,
  PRIMARY KEY (`idCaja`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cajageneral`
--

LOCK TABLES `cajageneral` WRITE;
/*!40000 ALTER TABLE `cajageneral` DISABLE KEYS */;
INSERT INTO `cajageneral` VALUES (24,'2018-03-15 00:00:00',943,29),(30,'2018-03-23 00:00:00',1299,35),(31,'2018-03-12 00:00:00',1264,36),(32,'2018-03-12 00:00:00',1514,37);
/*!40000 ALTER TABLE `cajageneral` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cuentas`
--

DROP TABLE IF EXISTS `cuentas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `cuentas` (
  `idCuenta` int(11) NOT NULL AUTO_INCREMENT,
  `descCuenta` varchar(45) NOT NULL,
  `idUsuario` int(11) DEFAULT NULL,
  PRIMARY KEY (`idCuenta`),
  KEY `fk_Cuenta_Usu` (`idUsuario`),
  CONSTRAINT `fk_CuentaUsuario` FOREIGN KEY (`idUsuario`) REFERENCES `usuarios` (`idUsuario`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cuentas`
--

LOCK TABLES `cuentas` WRITE;
/*!40000 ALTER TABLE `cuentas` DISABLE KEYS */;
INSERT INTO `cuentas` VALUES (1,'Cuenta Cred Ray',2);
/*!40000 ALTER TABLE `cuentas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `evactivopasivos`
--

DROP TABLE IF EXISTS `evactivopasivos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `evactivopasivos` (
  `idEvento` int(11) NOT NULL AUTO_INCREMENT,
  `descEvento` varchar(45) NOT NULL,
  `tipoEvento` tinytext NOT NULL,
  PRIMARY KEY (`idEvento`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `evactivopasivos`
--

LOCK TABLES `evactivopasivos` WRITE;
/*!40000 ALTER TABLE `evactivopasivos` DISABLE KEYS */;
INSERT INTO `evactivopasivos` VALUES (2,'Saldo Inicnal','C'),(3,'Endesa','D'),(4,'Ayuntamiento Sevilla','C'),(5,'Gas Natural','D'),(7,'Supermercado','D'),(8,'Entrada en Diñero','C'),(9,'Ayuntamiento Deb','D');
/*!40000 ALTER TABLE `evactivopasivos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `movimientos`
--

DROP TABLE IF EXISTS `movimientos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `movimientos` (
  `idMov` int(11) NOT NULL AUTO_INCREMENT,
  `descMov` varchar(45) NOT NULL,
  `fecha` datetime DEFAULT NULL,
  `valor` decimal(8,2) unsigned,
  `idEvento` int(11) NOT NULL,
  `idCuenta` int(11) NOT NULL,
  PRIMARY KEY (`idMov`),
  KEY `fk_Mov_Evento` (`idEvento`),
  KEY `fk_Mov_Cuenta` (`idCuenta`),
  CONSTRAINT `fk_MovEvento` FOREIGN KEY (`idEvento`) REFERENCES `evactivopasivos` (`idEvento`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `fk_MovCuenta` FOREIGN KEY (`idCuenta`) REFERENCES `cuentas` (`idCuenta`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `movimientos`
--

LOCK TABLES `movimientos` WRITE;
/*!40000 ALTER TABLE `movimientos` DISABLE KEYS */;
INSERT INTO `movimientos` VALUES (29,'pagto luz','2018-03-15 00:00:00',124,3,3,1),(35,'Entrada de Usu','2018-03-23 00:00:00',250,4,8,1),(36,'Pagto Tasa','2018-03-12 00:00:00',35,4,9,2),(37,'Entrada Usu','2018-03-12 00:00:00',250,4,8,2);
/*!40000 ALTER TABLE `movimientos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuarios`
--

DROP TABLE IF EXISTS `usuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `usuarios` (
  `idUsuario` int(11) NOT NULL AUTO_INCREMENT,
  `login` varchar(30) NOT NULL,
  `password` varchar(45) NOT NULL,
  `nombre` varchar(45) NOT NULL,
  `tipo` tinyint(4) NOT NULL,
  `situacion` tinyint(4) NOT NULL,
  `ruta` varchar(150) DEFAULT NULL,
  `foto` blob DEFAULT NULL,  
  PRIMARY KEY (`idUsuario`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuarios`
--

LOCK TABLES `usuarios` WRITE;
/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
INSERT INTO `usuarios` VALUES (2,'a','VM0VReIH1vc=','Ray',1,1,'~//temp//avatar2.png',null),(3,'usu','9kcKGflZ8Is=','Usuario Normal',2,1,'~//temp//caraf1.png',null),(4,'admin','N1Jgr72zw/8=','Administrador',1,1,'~//temp//cara1.png',null),(5,'des','YjESp253KeA=','Desabilitado',1,0,'~//temp//murilo.jpg',null);
/*!40000 ALTER TABLE `usuarios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `logs`
--

DROP TABLE IF EXISTS `logs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `logs` (
  `idLog` int(11) NOT NULL AUTO_INCREMENT,
  `logDesc` varchar(80) NOT NULL,
  `fechaLog` timestamp default current_timestamp,
  PRIMARY KEY (`idLog`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Dumping events for database 'homefinance'
--

--
-- Dumping routines for database 'homefinance'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-03-12 10:49:36
