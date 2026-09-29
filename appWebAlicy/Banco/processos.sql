CREATE DATABASE IF NOT EXISTS pds_app_web;
USE pds_app_web;

CREATE TABLE IF NOT EXISTS processos (
    id INT AUTO_INCREMENT PRIMARY KEY,
    numero_pro VARCHAR(200) NOT NULL,
    data_pro DATE NOT NULL,
    interessado_pro VARCHAR(200) NOT NULL,
    assunto_pro VARCHAR(300) NOT NULL,
    descricao_pro VARCHAR(2000),
    situacao_pro VARCHAR(50) NOT NULL DEFAULT 'Aberto'
);
