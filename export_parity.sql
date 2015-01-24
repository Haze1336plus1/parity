-- phpMyAdmin SQL Dump
-- version 4.0.4.1
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Erstellungszeit: 31. Aug 2014 um 17:51
-- Server Version: 5.6.11
-- PHP-Version: 5.5.3

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Datenbank: `parity`
--
CREATE DATABASE IF NOT EXISTS `parity` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `parity`;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `accounts`
--

CREATE TABLE IF NOT EXISTS `accounts` (
  `account_id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(32) NOT NULL,
  `password` varchar(32) NOT NULL,
  `nickname` varchar(32) NOT NULL,
  `email` varchar(255) NOT NULL,
  `online` tinyint(1) NOT NULL DEFAULT '0',
  `session` int(8) DEFAULT NULL,
  `mac_address` varchar(12) NOT NULL,
  `authlevel` tinyint(4) NOT NULL DEFAULT '0',
  PRIMARY KEY (`account_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=103 ;

--
-- Daten für Tabelle `accounts`
--

INSERT INTO `accounts` (`account_id`, `username`, `password`, `nickname`, `email`, `online`, `session`, `mac_address`, `authlevel`) VALUES
(1, 'turtle', '6bb6142b07cc8d87952a2a0beccc1fd5', 'AmazingTurtle', 'theblackhat1337@gmail.com', 0, NULL, '', 2),
(2, 'turtleBitch', 'cc03e747a6afbbcbf8be7668acfebee5', 'test123', 'test@123.com', 0, NULL, '', 0),
(3, 'testUser0', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick0', 'test0@mail.com', 0, NULL, '', 0),
(4, 'testUser1', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick1', 'test1@mail.com', 0, NULL, '', 0),
(5, 'testUser2', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick2', 'test2@mail.com', 0, NULL, '', 0),
(6, 'testUser3', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick3', 'test3@mail.com', 0, NULL, '', 0),
(7, 'testUser4', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick4', 'test4@mail.com', 0, NULL, '', 0),
(8, 'testUser5', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick5', 'test5@mail.com', 0, NULL, '', 0),
(9, 'testUser6', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick6', 'test6@mail.com', 0, NULL, '', 0),
(10, 'testUser7', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick7', 'test7@mail.com', 0, NULL, '', 0),
(11, 'testUser8', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick8', 'test8@mail.com', 0, NULL, '', 0),
(12, 'testUser9', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick9', 'test9@mail.com', 0, NULL, '', 0),
(13, 'testUser10', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick10', 'test10@mail.com', 0, NULL, '', 0),
(14, 'testUser11', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick11', 'test11@mail.com', 0, NULL, '', 0),
(15, 'testUser12', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick12', 'test12@mail.com', 0, NULL, '', 0),
(16, 'testUser13', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick13', 'test13@mail.com', 0, NULL, '', 0),
(17, 'testUser14', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick14', 'test14@mail.com', 0, NULL, '', 0),
(18, 'testUser15', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick15', 'test15@mail.com', 0, NULL, '', 0),
(19, 'testUser16', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick16', 'test16@mail.com', 0, NULL, '', 0),
(20, 'testUser17', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick17', 'test17@mail.com', 0, NULL, '', 0),
(21, 'testUser18', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick18', 'test18@mail.com', 0, NULL, '', 0),
(22, 'testUser19', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick19', 'test19@mail.com', 0, NULL, '', 0),
(23, 'testUser20', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick20', 'test20@mail.com', 0, NULL, '', 0),
(24, 'testUser21', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick21', 'test21@mail.com', 0, NULL, '', 0),
(25, 'testUser22', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick22', 'test22@mail.com', 0, NULL, '', 0),
(26, 'testUser23', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick23', 'test23@mail.com', 0, NULL, '', 0),
(27, 'testUser24', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick24', 'test24@mail.com', 0, NULL, '', 0),
(28, 'testUser25', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick25', 'test25@mail.com', 0, NULL, '', 0),
(29, 'testUser26', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick26', 'test26@mail.com', 0, NULL, '', 0),
(30, 'testUser27', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick27', 'test27@mail.com', 0, NULL, '', 0),
(31, 'testUser28', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick28', 'test28@mail.com', 0, NULL, '', 0),
(32, 'testUser29', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick29', 'test29@mail.com', 0, NULL, '', 0),
(33, 'testUser30', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick30', 'test30@mail.com', 0, NULL, '', 0),
(34, 'testUser31', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick31', 'test31@mail.com', 0, NULL, '', 0),
(35, 'testUser32', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick32', 'test32@mail.com', 0, NULL, '', 0),
(36, 'testUser33', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick33', 'test33@mail.com', 0, NULL, '', 0),
(37, 'testUser34', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick34', 'test34@mail.com', 0, NULL, '', 0),
(38, 'testUser35', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick35', 'test35@mail.com', 0, NULL, '', 0),
(39, 'testUser36', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick36', 'test36@mail.com', 0, NULL, '', 0),
(40, 'testUser37', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick37', 'test37@mail.com', 0, NULL, '', 0),
(41, 'testUser38', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick38', 'test38@mail.com', 0, NULL, '', 0),
(42, 'testUser39', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick39', 'test39@mail.com', 0, NULL, '', 0),
(43, 'testUser40', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick40', 'test40@mail.com', 0, NULL, '', 0),
(44, 'testUser41', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick41', 'test41@mail.com', 0, NULL, '', 0),
(45, 'testUser42', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick42', 'test42@mail.com', 0, NULL, '', 0),
(46, 'testUser43', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick43', 'test43@mail.com', 0, NULL, '', 0),
(47, 'testUser44', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick44', 'test44@mail.com', 0, NULL, '', 0),
(48, 'testUser45', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick45', 'test45@mail.com', 0, NULL, '', 0),
(49, 'testUser46', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick46', 'test46@mail.com', 0, NULL, '', 0),
(50, 'testUser47', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick47', 'test47@mail.com', 0, NULL, '', 0),
(51, 'testUser48', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick48', 'test48@mail.com', 0, NULL, '', 0),
(52, 'testUser49', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick49', 'test49@mail.com', 0, NULL, '', 0),
(53, 'testUser50', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick50', 'test50@mail.com', 0, NULL, '', 0),
(54, 'testUser51', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick51', 'test51@mail.com', 0, NULL, '', 0),
(55, 'testUser52', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick52', 'test52@mail.com', 0, NULL, '', 0),
(56, 'testUser53', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick53', 'test53@mail.com', 0, NULL, '', 0),
(57, 'testUser54', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick54', 'test54@mail.com', 0, NULL, '', 0),
(58, 'testUser55', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick55', 'test55@mail.com', 0, NULL, '', 0),
(59, 'testUser56', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick56', 'test56@mail.com', 0, NULL, '', 0),
(60, 'testUser57', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick57', 'test57@mail.com', 0, NULL, '', 0),
(61, 'testUser58', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick58', 'test58@mail.com', 0, NULL, '', 0),
(62, 'testUser59', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick59', 'test59@mail.com', 0, NULL, '', 0),
(63, 'testUser60', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick60', 'test60@mail.com', 0, NULL, '', 0),
(64, 'testUser61', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick61', 'test61@mail.com', 0, NULL, '', 0),
(65, 'testUser62', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick62', 'test62@mail.com', 0, NULL, '', 0),
(66, 'testUser63', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick63', 'test63@mail.com', 0, NULL, '', 0),
(67, 'testUser64', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick64', 'test64@mail.com', 0, NULL, '', 0),
(68, 'testUser65', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick65', 'test65@mail.com', 0, NULL, '', 0),
(69, 'testUser66', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick66', 'test66@mail.com', 0, NULL, '', 0),
(70, 'testUser67', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick67', 'test67@mail.com', 0, NULL, '', 0),
(71, 'testUser68', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick68', 'test68@mail.com', 0, NULL, '', 0),
(72, 'testUser69', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick69', 'test69@mail.com', 0, NULL, '', 0),
(73, 'testUser70', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick70', 'test70@mail.com', 0, NULL, '', 0),
(74, 'testUser71', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick71', 'test71@mail.com', 0, NULL, '', 0),
(75, 'testUser72', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick72', 'test72@mail.com', 0, NULL, '', 0),
(76, 'testUser73', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick73', 'test73@mail.com', 0, NULL, '', 0),
(77, 'testUser74', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick74', 'test74@mail.com', 0, NULL, '', 0),
(78, 'testUser75', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick75', 'test75@mail.com', 0, NULL, '', 0),
(79, 'testUser76', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick76', 'test76@mail.com', 0, NULL, '', 0),
(80, 'testUser77', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick77', 'test77@mail.com', 0, NULL, '', 0),
(81, 'testUser78', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick78', 'test78@mail.com', 0, NULL, '', 0),
(82, 'testUser79', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick79', 'test79@mail.com', 0, NULL, '', 0),
(83, 'testUser80', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick80', 'test80@mail.com', 0, NULL, '', 0),
(84, 'testUser81', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick81', 'test81@mail.com', 0, NULL, '', 0),
(85, 'testUser82', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick82', 'test82@mail.com', 0, NULL, '', 0),
(86, 'testUser83', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick83', 'test83@mail.com', 0, NULL, '', 0),
(87, 'testUser84', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick84', 'test84@mail.com', 0, NULL, '', 0),
(88, 'testUser85', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick85', 'test85@mail.com', 0, NULL, '', 0),
(89, 'testUser86', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick86', 'test86@mail.com', 0, NULL, '', 0),
(90, 'testUser87', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick87', 'test87@mail.com', 0, NULL, '', 0),
(91, 'testUser88', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick88', 'test88@mail.com', 0, NULL, '', 0),
(92, 'testUser89', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick89', 'test89@mail.com', 0, NULL, '', 0),
(93, 'testUser90', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick90', 'test90@mail.com', 0, NULL, '', 0),
(94, 'testUser91', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick91', 'test91@mail.com', 0, NULL, '', 0),
(95, 'testUser92', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick92', 'test92@mail.com', 0, NULL, '', 0),
(96, 'testUser93', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick93', 'test93@mail.com', 0, NULL, '', 0),
(97, 'testUser94', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick94', 'test94@mail.com', 0, NULL, '', 0),
(98, 'testUser95', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick95', 'test95@mail.com', 0, NULL, '', 0),
(99, 'testUser96', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick96', 'test96@mail.com', 0, NULL, '', 0),
(100, 'testUser97', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick97', 'test97@mail.com', 0, NULL, '', 0),
(101, 'testUser98', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick98', 'test98@mail.com', 0, NULL, '', 0),
(102, 'testUser99', 'b62a565853f37fb1ec1efc287bfcebf9', 'testNick99', 'test99@mail.com', 0, NULL, '', 0);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `mac_bans`
--

CREATE TABLE IF NOT EXISTS `mac_bans` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `mac` varchar(12) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `news`
--

CREATE TABLE IF NOT EXISTS `news` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `poster_id` int(11) NOT NULL,
  `date` datetime NOT NULL,
  `type` varchar(16) NOT NULL,
  `title` varchar(32) NOT NULL,
  `content` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=2 ;

--
-- Daten für Tabelle `news`
--

INSERT INTO `news` (`id`, `poster_id`, `date`, `type`, `title`, `content`) VALUES
(1, 1, '2014-08-04 19:28:03', 'NOTICE', 'test', '123');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `outbox`
--

CREATE TABLE IF NOT EXISTS `outbox` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account_id` int(11) NOT NULL,
  `code` varchar(4) NOT NULL,
  `purchase` datetime NOT NULL,
  `duration` int(11) NOT NULL COMMENT 'in days',
  `from` varchar(32) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

--
-- Daten für Tabelle `outbox`
--

INSERT INTO `outbox` (`id`, `account_id`, `code`, `purchase`, `duration`, `from`) VALUES
(2, 1, 'DB14', '2014-08-03 17:16:14', 30, 'AmazingTurtle');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `stats`
--

CREATE TABLE IF NOT EXISTS `stats` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `registerDate` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=103 ;

--
-- Daten für Tabelle `stats`
--

INSERT INTO `stats` (`id`, `registerDate`) VALUES
(1, '2014-07-29 12:00:00'),
(2, '2014-07-29 12:00:00'),
(3, '2014-07-29 12:00:00'),
(4, '2014-07-29 12:00:00'),
(5, '2014-07-29 12:00:00'),
(6, '2014-07-29 12:00:00'),
(7, '2014-07-29 12:00:00'),
(8, '2014-07-29 12:00:00'),
(9, '2014-07-29 12:00:00'),
(10, '2014-07-29 12:00:00'),
(11, '2014-07-29 12:00:00'),
(12, '2014-07-29 12:00:00'),
(13, '2014-07-29 12:00:00'),
(14, '2014-07-29 12:00:00'),
(15, '2014-07-29 12:00:00'),
(16, '2014-07-29 12:00:00'),
(17, '2014-07-29 12:00:00'),
(18, '2014-07-29 12:00:00'),
(19, '2014-07-29 12:00:00'),
(20, '2014-07-29 12:00:00'),
(21, '2014-07-29 12:00:00'),
(22, '2014-07-29 12:00:00'),
(23, '2014-07-29 12:00:00'),
(24, '2014-07-29 12:00:00'),
(25, '2014-07-29 12:00:00'),
(26, '2014-07-29 12:00:00'),
(27, '2014-07-29 12:00:00'),
(28, '2014-07-29 12:00:00'),
(29, '2014-07-29 12:00:00'),
(30, '2014-07-29 12:00:00'),
(31, '2014-07-29 12:00:00'),
(32, '2014-07-29 12:00:00'),
(33, '2014-07-29 12:00:00'),
(34, '2014-07-29 12:00:00'),
(35, '2014-07-29 12:00:00'),
(36, '2014-07-29 12:00:00'),
(37, '2014-07-29 12:00:00'),
(38, '2014-07-29 12:00:00'),
(39, '2014-07-29 12:00:00'),
(40, '2014-07-29 12:00:00'),
(41, '2014-07-29 12:00:00'),
(42, '2014-07-29 12:00:00'),
(43, '2014-07-29 12:00:00'),
(44, '2014-07-29 12:00:00'),
(45, '2014-07-29 12:00:00'),
(46, '2014-07-29 12:00:00'),
(47, '2014-07-29 12:00:00'),
(48, '2014-07-29 12:00:00'),
(49, '2014-07-29 12:00:00'),
(50, '2014-07-30 12:57:16'),
(51, '2014-07-30 12:57:16'),
(52, '2014-07-30 12:57:17'),
(53, '2014-07-30 12:57:17'),
(54, '2014-07-30 12:57:17'),
(55, '2014-07-30 12:57:17'),
(56, '2014-07-30 12:57:17'),
(57, '2014-07-30 12:57:17'),
(58, '2014-07-30 12:57:17'),
(59, '2014-07-30 12:57:17'),
(60, '2014-07-30 12:57:17'),
(61, '2014-07-30 12:57:17'),
(62, '2014-07-30 12:57:17'),
(63, '2014-07-30 12:57:17'),
(64, '2014-07-30 12:57:17'),
(65, '2014-07-30 12:57:17'),
(66, '2014-07-30 12:57:17'),
(67, '2014-07-30 12:57:17'),
(68, '2014-07-30 12:57:17'),
(69, '2014-07-30 12:57:17'),
(70, '2014-07-30 12:57:17'),
(71, '2014-07-30 12:57:17'),
(72, '2014-07-30 12:57:17'),
(73, '2014-07-30 12:57:17'),
(74, '2014-07-30 12:57:17'),
(75, '2014-07-30 12:57:17'),
(76, '2014-07-30 12:57:17'),
(77, '2014-07-30 12:57:17'),
(78, '2014-07-30 12:57:17'),
(79, '2014-07-30 12:57:17'),
(80, '2014-07-30 12:57:17'),
(81, '2014-07-30 12:57:17'),
(82, '2014-07-30 12:57:17'),
(83, '2014-07-30 12:57:17'),
(84, '2014-07-30 12:57:17'),
(85, '2014-07-30 12:57:17'),
(86, '2014-07-30 12:57:17'),
(87, '2014-07-30 12:57:17'),
(88, '2014-07-30 12:57:17'),
(89, '2014-07-30 12:57:17'),
(90, '2014-07-30 12:57:17'),
(91, '2014-07-30 12:57:17'),
(92, '2014-07-30 12:57:17'),
(93, '2014-07-30 12:57:17'),
(94, '2014-07-30 12:57:17'),
(95, '2014-07-30 12:57:17'),
(96, '2014-07-30 12:57:17'),
(97, '2014-07-30 12:57:17'),
(98, '2014-07-30 12:57:17'),
(99, '2014-07-30 12:57:17'),
(100, '2014-07-30 12:57:17'),
(101, '2014-07-30 12:57:17'),
(102, '2014-07-30 12:57:17');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `ticket_data`
--

CREATE TABLE IF NOT EXISTS `ticket_data` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `ticket_id` int(11) NOT NULL,
  `poster_id` int(11) NOT NULL,
  `message` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

--
-- Daten für Tabelle `ticket_data`
--

INSERT INTO `ticket_data` (`id`, `ticket_id`, `poster_id`, `message`) VALUES
(1, 3, 1, 'so whatsup?\ndid you ever care?\nhow can i help?'),
(2, 3, 98, 'asd');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `tickets`
--

CREATE TABLE IF NOT EXISTS `tickets` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `title` varchar(64) NOT NULL,
  `state` enum('pending','waiting','solved') NOT NULL DEFAULT 'pending',
  `poster_id` int(11) NOT NULL,
  `last_commit` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=4 ;

--
-- Daten für Tabelle `tickets`
--

INSERT INTO `tickets` (`id`, `title`, `state`, `poster_id`, `last_commit`) VALUES
(1, 'test', 'waiting', 1, '2014-08-01 00:00:00'),
(2, 'u wot ferble', 'pending', 57, '2014-08-01 04:00:00'),
(3, 'invisible bug', 'solved', 98, '2014-08-01 04:08:24');
--
-- Datenbank: `parity_game1`
--
CREATE DATABASE IF NOT EXISTS `parity_game1` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `parity_game1`;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `character`
--

CREATE TABLE IF NOT EXISTS `character` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account_id` int(11) NOT NULL,
  `class` tinyint(4) NOT NULL,
  `slot1` int(11) NOT NULL DEFAULT '-1',
  `slot2` int(11) NOT NULL DEFAULT '-1',
  `slot3` int(11) NOT NULL DEFAULT '-1',
  `slot4` int(11) NOT NULL DEFAULT '-1',
  `slot5` int(11) NOT NULL DEFAULT '-1',
  `slot6` int(11) NOT NULL DEFAULT '-1',
  `slot7` int(11) NOT NULL DEFAULT '-1',
  `slot8` int(11) NOT NULL DEFAULT '-1',
  `slot9` int(11) NOT NULL DEFAULT '-1',
  `slot10` int(11) NOT NULL DEFAULT '-1',
  `slot11` int(11) NOT NULL DEFAULT '-1',
  `slot12` int(11) NOT NULL DEFAULT '-1',
  `slot13` int(11) NOT NULL DEFAULT '-1',
  `slot14` int(11) NOT NULL DEFAULT '-1',
  `slot15` int(11) NOT NULL DEFAULT '-1',
  `slot16` int(11) NOT NULL DEFAULT '-1',
  `slot17` int(11) NOT NULL DEFAULT '-1',
  `slot18` int(11) NOT NULL DEFAULT '-1',
  `slot19` int(11) NOT NULL DEFAULT '-1',
  `slot20` int(11) NOT NULL DEFAULT '-1',
  `slot21` int(11) NOT NULL DEFAULT '-1',
  `slot22` int(11) NOT NULL DEFAULT '-1',
  `slot23` int(11) NOT NULL DEFAULT '-1',
  `slot24` int(11) NOT NULL DEFAULT '-1',
  `slot25` int(11) NOT NULL DEFAULT '-1',
  `slot26` int(11) NOT NULL DEFAULT '-1',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=11 ;

--
-- Daten für Tabelle `character`
--

INSERT INTO `character` (`id`, `account_id`, `class`, `slot1`, `slot2`, `slot3`, `slot4`, `slot5`, `slot6`, `slot7`, `slot8`, `slot9`, `slot10`, `slot11`, `slot12`, `slot13`, `slot14`, `slot15`, `slot16`, `slot17`, `slot18`, `slot19`, `slot20`, `slot21`, `slot22`, `slot23`, `slot24`, `slot25`, `slot26`) VALUES
(1, 1, 0, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1),
(2, 1, 1, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1),
(3, 1, 2, 13, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1),
(4, 1, 3, 8, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1),
(5, 1, 4, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1),
(6, 2, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1),
(7, 2, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1),
(8, 2, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1),
(9, 2, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1),
(10, 2, 4, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `details`
--

CREATE TABLE IF NOT EXISTS `details` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `kills` int(11) NOT NULL,
  `deaths` int(11) NOT NULL,
  `experience` int(11) NOT NULL,
  `dinar` int(11) NOT NULL,
  `credits` int(11) NOT NULL,
  `premium` tinyint(4) NOT NULL,
  `premiumbegin` datetime NOT NULL,
  `premiumduration` int(11) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=103 ;

--
-- Daten für Tabelle `details`
--

INSERT INTO `details` (`id`, `kills`, `deaths`, `experience`, `dinar`, `credits`, `premium`, `premiumbegin`, `premiumduration`) VALUES
(1, 0, 0, 37757475, 114900, 40000, 4, '2014-08-03 17:18:24', 365),
(2, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:51:05', 0),
(3, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(4, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(5, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(6, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(7, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(8, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(9, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(10, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(11, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(12, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(13, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(14, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(15, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(16, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(17, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(18, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(19, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(20, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(21, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(22, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(23, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(24, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(25, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(26, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(27, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(28, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(29, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(30, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(31, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(32, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(33, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(34, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(35, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(36, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(37, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(38, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(39, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(40, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(41, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(42, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(43, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(44, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(45, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(46, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(47, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(48, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(49, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(50, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(51, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:16', 0),
(52, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(53, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(54, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(55, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(56, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(57, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(58, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(59, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(60, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(61, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(62, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(63, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(64, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(65, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(66, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(67, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(68, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(69, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(70, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(71, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(72, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(73, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(74, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(75, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(76, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(77, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(78, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(79, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(80, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(81, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(82, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(83, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(84, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(85, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(86, 0, 0, 0, 140000, 40000, 0, '2014-07-30 13:07:07', 0),
(87, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(88, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(89, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(90, 0, 0, 0, 140000, 40000, 0, '2014-07-30 13:07:50', 0),
(91, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(92, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(93, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(94, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(95, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(96, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(97, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(98, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(99, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(100, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(101, 0, 0, 0, 140000, 40000, 0, '2014-07-30 12:57:17', 0),
(102, 0, 0, 0, 140000, 40000, 0, '2014-08-03 10:08:01', 0);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `equipment`
--

CREATE TABLE IF NOT EXISTS `equipment` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account_id` int(11) NOT NULL,
  `class` tinyint(4) NOT NULL,
  `slot1` int(11) NOT NULL DEFAULT '-1',
  `slot2` int(11) NOT NULL DEFAULT '-1',
  `slot3` int(11) NOT NULL DEFAULT '-1',
  `slot4` int(11) NOT NULL DEFAULT '-1',
  `slot5` int(11) NOT NULL DEFAULT '-1',
  `slot6` int(11) NOT NULL DEFAULT '-1',
  `slot7` int(11) NOT NULL DEFAULT '-1',
  `slot8` int(11) NOT NULL DEFAULT '-1',
  `slot6change` int(11) NOT NULL DEFAULT '-1',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_2` (`id`),
  KEY `id` (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=11 ;

--
-- Daten für Tabelle `equipment`
--

INSERT INTO `equipment` (`id`, `account_id`, `class`, `slot1`, `slot2`, `slot3`, `slot4`, `slot5`, `slot6`, `slot7`, `slot8`, `slot6change`) VALUES
(1, 1, 0, 0, 0, 4, 0, 0, 0, 0, 0, 0),
(2, 1, 1, 0, 0, 4, 0, 0, 0, 0, 0, 0),
(3, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0),
(4, 1, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0),
(5, 1, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0),
(6, 2, 0, -1, -1, -1, -1, -1, -1, -1, -1, -1),
(7, 2, 1, -1, -1, -1, -1, -1, -1, -1, -1, -1),
(8, 2, 2, -1, -1, -1, -1, -1, -1, -1, -1, -1),
(9, 2, 3, -1, -1, -1, -1, -1, -1, -1, -1, -1),
(10, 2, 4, -1, -1, -1, -1, -1, -1, -1, -1, -1);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `items`
--

CREATE TABLE IF NOT EXISTS `items` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `account_id` int(11) NOT NULL,
  `code` varchar(4) NOT NULL,
  `purchase` datetime NOT NULL,
  `duration` int(11) NOT NULL COMMENT 'in days',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=18 ;

--
-- Daten für Tabelle `items`
--

INSERT INTO `items` (`id`, `account_id`, `code`, `purchase`, `duration`) VALUES
(2, 1, 'DB14', '2014-05-27 15:52:58', 381),
(4, 1, 'DD03', '2014-05-27 16:24:21', 134),
(8, 1, 'BA0E', '2014-06-07 23:29:57', 365),
(13, 1, 'BA08', '2014-06-08 00:16:21', 365),
(14, 2, 'DC64', '2014-06-04 00:00:00', 123),
(16, 1, 'BA28', '2014-07-09 00:37:48', 365),
(17, 1, 'BA23', '2014-07-09 00:38:27', 365);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
