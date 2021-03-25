CREATE DATABASE  IF NOT EXISTS `mychat` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `mychat`;
-- MySQL dump 10.13  Distrib 8.0.11, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: mychat
-- ------------------------------------------------------
-- Server version	8.0.11

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `chats`
--

DROP TABLE IF EXISTS `chats`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `chats` (
  `chat_id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(128) NOT NULL,
  `creator_id` int(11) NOT NULL,
  PRIMARY KEY (`chat_id`),
  KEY `fk_chats_users_creator_id_idx` (`creator_id`),
  CONSTRAINT `fk_chats_users_creator_id` FOREIGN KEY (`creator_id`) REFERENCES `users` (`user_id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=26 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `chats`
--

LOCK TABLES `chats` WRITE;
/*!40000 ALTER TABLE `chats` DISABLE KEYS */;
INSERT INTO `chats` VALUES (1,'Общий',1),(25,'Первый созданный чат!',11);
/*!40000 ALTER TABLE `chats` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`yuriy`@`%`*/ /*!50003 TRIGGER `chats_AFTER_INSERT` AFTER INSERT ON `chats` FOR EACH ROW BEGIN
	INSERT INTO `mychat`.`chats_members` (`user_id`, `chat_id`) VALUES (NEW.`creator_id`, NEW.`chat_id`);
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Table structure for table `chats_members`
--

DROP TABLE IF EXISTS `chats_members`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `chats_members` (
  `user_id` int(11) NOT NULL,
  `chat_id` int(11) NOT NULL,
  PRIMARY KEY (`user_id`,`chat_id`),
  KEY `chats_members_chats_chat_id_idx` (`chat_id`),
  CONSTRAINT `fk_chats_members_chats_chat_id` FOREIGN KEY (`chat_id`) REFERENCES `chats` (`chat_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_chats_members_users_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`user_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `chats_members`
--

LOCK TABLES `chats_members` WRITE;
/*!40000 ALTER TABLE `chats_members` DISABLE KEYS */;
INSERT INTO `chats_members` VALUES (1,1),(10,1),(11,1),(10,25),(11,25);
/*!40000 ALTER TABLE `chats_members` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `messages`
--

DROP TABLE IF EXISTS `messages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `messages` (
  `message_id` int(11) NOT NULL AUTO_INCREMENT,
  `chat_id` int(11) NOT NULL,
  `sender_id` int(11) NOT NULL,
  `content` varchar(512) NOT NULL,
  `sending_datetime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`message_id`),
  KEY `fk_messages_users_sender_id_idx` (`sender_id`),
  KEY `fk_messages_chats_chat_id_idx` (`chat_id`),
  CONSTRAINT `fk_messages_chats_chat_id` FOREIGN KEY (`chat_id`) REFERENCES `chats` (`chat_id`) ON UPDATE CASCADE,
  CONSTRAINT `fk_messages_users_sender_id` FOREIGN KEY (`sender_id`) REFERENCES `users` (`user_id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=94 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `messages`
--

LOCK TABLES `messages` WRITE;
/*!40000 ALTER TABLE `messages` DISABLE KEYS */;
INSERT INTO `messages` VALUES (87,1,10,'Первое сообщение!','2021-03-25 15:56:21'),(88,1,10,'Первое сообщение с вложением!','2021-03-25 15:56:39'),(89,1,11,'Первое сообщение с другого аккаунта!','2021-03-25 15:57:33'),(90,1,11,'Первое сообщение с вложением с другого аккаунта!','2021-03-25 15:57:54'),(91,25,11,'Первое сообщение в созданном чате!','2021-03-25 15:58:25'),(92,25,11,'Второе сообщение и т.д.','2021-03-25 16:00:52'),(93,25,10,'Ответ','2021-03-25 16:01:49');
/*!40000 ALTER TABLE `messages` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `messages_attachments`
--

DROP TABLE IF EXISTS `messages_attachments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `messages_attachments` (
  `attachment_id` int(11) NOT NULL AUTO_INCREMENT,
  `message_id` int(11) NOT NULL,
  `path` varchar(512) NOT NULL,
  PRIMARY KEY (`attachment_id`),
  KEY `fk_messages_attachments_messages_message_id_idx` (`message_id`),
  CONSTRAINT `fk_messages_attachments_messages_message_id` FOREIGN KEY (`message_id`) REFERENCES `messages` (`message_id`) ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `messages_attachments`
--

LOCK TABLES `messages_attachments` WRITE;
/*!40000 ALTER TABLE `messages_attachments` DISABLE KEYS */;
INSERT INTO `messages_attachments` VALUES (1,88,'/img/cellar-interior-laundry-inside-the-basement-in-cartoon-style_1441-1756.jpg'),(2,90,'/img/thumb_585.jpeg'),(3,92,'/img/9914361741342.jpg');
/*!40000 ALTER TABLE `messages_attachments` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `messages_statuses`
--

DROP TABLE IF EXISTS `messages_statuses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `messages_statuses` (
  `message_id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `is_readed` tinyint(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`message_id`,`user_id`,`is_readed`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `messages_statuses`
--

LOCK TABLES `messages_statuses` WRITE;
/*!40000 ALTER TABLE `messages_statuses` DISABLE KEYS */;
/*!40000 ALTER TABLE `messages_statuses` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `users` (
  `user_id` int(11) NOT NULL AUTO_INCREMENT,
  `first_name` varchar(45) NOT NULL,
  `middle_name` varchar(45) NOT NULL,
  `last_name` varchar(45) DEFAULT NULL,
  `login` varchar(45) NOT NULL,
  `password` varchar(45) NOT NULL,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `login_UNIQUE` (`login`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'system','system','system','BexQE2Qw+PA=','BexQE2Qw+PA='),(10,'Юрий','Сергеевич','Падерин','gw7bvVc95N0=','gw7bvVc95N0='),(11,'Мария','Валдисовна','Оруп','h0Dg5JgSp1E=','h0Dg5JgSp1E=');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 DEFINER=`yuriy`@`%`*/ /*!50003 TRIGGER `users_AFTER_INSERT` AFTER INSERT ON `users` FOR EACH ROW BEGIN
	INSERT INTO `mychat`.`chats_members` (`user_id`, `chat_id`) VALUES (NEW.`user_id`, 1);
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Dumping events for database 'mychat'
--

--
-- Dumping routines for database 'mychat'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-03-25 16:17:46
