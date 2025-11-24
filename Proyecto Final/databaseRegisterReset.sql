USE [VeterinariaDB]
GO

-- =========================================================
-- PASO 1: DESHABILITAR CLAVES FORÁNEAS (Evita errores de DELETE)
-- Usamos ALTER TABLE NOCHECK CONSTRAINT para garantizar que DELETE funcione sin errores de FK.
-- =========================================================

-- Deshabilitar FKs en Tablas Hijas
ALTER TABLE [dbo].[Turno] NOCHECK CONSTRAINT ALL;
ALTER TABLE [dbo].[Evento] NOCHECK CONSTRAINT ALL;
ALTER TABLE [dbo].[Mascota] NOCHECK CONSTRAINT [FK_Mascota_Clientes_ClienteId];
GO

-- =========================================================
-- PASO 2: LIMPIAR DATOS (Usando DELETE para evitar la restricción de TRUNCATE)
-- Limpiamos de dependiente a principal.
-- =========================================================

DELETE FROM [Turno];
DELETE FROM [Evento];
DELETE FROM [Usuarios]; 

DELETE FROM [Mascota];
DELETE FROM [Veterinario];
DELETE FROM [Clientes];
GO

-- =========================================================
-- PASO 3: REINICIAR CONTADORES DE IDENTIDAD (IDENTITY)
-- Esto logra el efecto de TRUNCATE (IDs reinician a 1)
-- =========================================================

DBCC CHECKIDENT ('Turno', RESEED, 0);
DBCC CHECKIDENT ('Evento', RESEED, 0);
DBCC CHECKIDENT ('Mascota', RESEED, 0);
DBCC CHECKIDENT ('Veterinario', RESEED, 0);
DBCC CHECKIDENT ('Clientes', RESEED, 0);
DBCC CHECKIDENT ('Usuarios', RESEED, 0);
GO

-- =========================================================
-- PASO 4: HABILITAR CLAVES FORÁNEAS
-- =========================================================

-- Habilitar FKs en Tablas Hijas
ALTER TABLE [dbo].[Mascota] CHECK CONSTRAINT [FK_Mascota_Clientes_ClienteId];
ALTER TABLE [dbo].[Evento] CHECK CONSTRAINT ALL;
ALTER TABLE [dbo].[Turno] CHECK CONSTRAINT ALL;
GO

-- =========================================================
-- PASO 5: INSERTAR DATOS DE PRUEBA (Datos originales)
-- =========================================================

-- ------------------------------
-- A) Clientes (10 Registros)
-- ID comenzarán en 1
-- ------------------------------
INSERT INTO Clientes (DNI, Nombre, Apellido, Email, Telefono, Direccion) VALUES
(10010011, 'Ana', 'García', 'ana.g@mail.com', 1123456701, 'Calle Falsa 123, Piso 1'),
(10010012, 'Juan', 'Pérez', 'juan.p@mail.com', 1123456702, 'Av. Siempre Viva 456'),
(10010013, 'Laura', 'Martínez', 'laura.m@mail.com', 1123456703, 'Ronda de Triana 10'),
(10010014, 'Carlos', 'López', 'carlos.l@mail.com', 1123456704, 'Gran Vía 55, Puerta 3'),
(10010015, 'Sofía', 'Rodríguez', 'sofia.r@mail.com', 1123456705, 'Elm Street 789'),
(10010016, 'David', 'Sánchez', 'david.s@mail.com', 1123456706, 'Calle Mayor 200'),
(10010017, 'Elena', 'Torres', 'elena.t@mail.com', 1123456707, 'Paseo Colón 15'),
(10010018, 'Miguel', 'Herrera', 'miguel.h@mail.com', 1123456708, 'Avenida del Sol 1'),
(10010019, 'Lucía', 'Ruiz', 'lucia.r@mail.com', 1123456709, 'Plaza España 5'),
(10010020, 'Pedro', 'Blanco', 'pedro.b@mail.com', 1123456710, 'Calle Luna 10');
GO

-- ------------------------------
-- B) Veterinarios (4 Registros)
-- ID comenzarán en 1
-- ------------------------------
INSERT INTO Veterinario (DNI, Nombre, Apellido, Matricula, Telefono, Direccion, Especialidad) VALUES
(20020011, 'Dr. Jorge', 'Acosta', 'MV-1001', 1133445501, 'Consultorio A', 'General'),
(20020012, 'Dra. María', 'Núñez', 'MV-1002', 1133445502, 'Consultorio B', 'Cirugía'),
(20020013, 'Dr. Ramiro', 'Vidal', 'MV-1003', 1133445503, 'Consultorio C', 'Exóticos'),
(20020014, 'Dra. Carolina', 'Flores', 'MV-1004', 1133445504, 'Consultorio D', 'Dermatología');
GO

-- ------------------------------
-- C) Mascotas (22 Registros)
-- ID comenzarán en 1
-- ------------------------------
INSERT INTO Mascota (ClienteId, Nombre, Especie, Raza, Peso, Vivo, FechaNacimiento) VALUES
(1, 'Luna', 'Canino', 'Labrador', 30.5, 1, '2020-05-15'), 
(2, 'Max', 'Canino', 'Pastor Alemán', 35.0, 1, '2019-01-20'), 
(2, 'Milo', 'Felino', 'Siamés', 4.5, 1, '2021-03-01'), 
(3, 'Copo', 'Felino', 'Persa', 5.2, 1, '2022-07-10'), 
(4, 'Bella', 'Canino', 'Golden', 28.0, 1, '2018-11-25'), 
(4, 'Toby', 'Canino', 'Poodle', 8.1, 1, '2023-04-03'), 
(4, 'Coco', 'Felino', 'Mestizo', 4.0, 1, '2024-01-12'), 
(5, 'Rex', 'Reptil', 'Iguana', 1.2, 1, '2020-02-02'), 
(5, 'Zoe', 'Roedor', 'Hámster', 0.1, 1, '2025-01-10'), 
(6, 'Simba', 'Felino', 'Maine Coon', 7.5, 1, '2017-09-09'), 
(7, 'Baloo', 'Canino', 'Boxer', 32.0, 1, '2016-06-01'), 
(7, 'Kira', 'Canino', 'Pitbull', 25.5, 1, '2021-08-18'), 
(8, 'Mia', 'Felino', 'Angora', 3.8, 1, '2023-11-11'), 
(8, 'Leo', 'Canino', 'Dálmata', 22.0, 1, '2024-05-30'), 
(8, 'Paco', 'Ave', 'Loro', 0.4, 1, '2022-10-05'), 
(9, 'Nala', 'Canino', 'Chihuahua', 3.0, 1, '2020-03-22'), 
(10, 'Rocky', 'Canino', 'Doberman', 40.0, 1, '2017-12-01'), 
(10, 'Chispa', 'Felino', 'Mestizo', 4.1, 1, '2023-09-15'), 
(10, 'Zeus', 'Roedor', 'Conejo', 2.0, 1, '2024-02-28'), 
(1, 'Gato', 'Felino', 'Mestizo', 4.0, 1, '2024-03-01'), 
(3, 'Kiwi', 'Ave', 'Periquito', 0.2, 1, '2024-06-15'), 
(6, 'Fido', 'Canino', 'Cocker', 15.0, 1, '2022-12-01');
GO

-- ------------------------------
-- D) Eventos (Suma entre 1 a 5 por mascota)
-- ------------------------------
INSERT INTO Evento (TipoEvento, Detalle, FechaHorario, MascotaId, VeterinarioId) VALUES
('Vacunación', 'Vacuna Antirrábica anual.', '2024-05-15 10:00:00', 1, 1),
('Desparasitación', 'Comprimido antiparasitario interno.', '2024-11-01 11:30:00', 1, 2),
('Consulta', 'Chequeo general de senior.', '2024-02-01 14:00:00', 2, 3),
('Laboratorio', 'Análisis de sangre y orina.', '2024-03-10 09:00:00', 2, 4),
('Tratamiento', 'Inicio de tratamiento articular.', '2024-04-15 10:30:00', 2, 3),
('Vacunación', 'Triple felina de refuerzo.', '2024-04-01 13:00:00', 3, 2),
('Consulta', 'Control de peso por sobrepeso leve.', '2025-01-05 15:00:00', 3, 1),
('Vacunación', 'Triple felina.', '2023-01-15 10:00:00', 4, 1),
('Consulta', 'Revisión ocular por lagrimeo.', '2023-03-20 11:00:00', 4, 4),
('Desparasitación', 'Spot-on antipulgas.', '2023-09-01 17:00:00', 4, 1),
('Laboratorio', 'Test de FIV/Leucemia Felina.', '2024-02-10 10:00:00', 4, 2),
('Consulta', 'Chequeo de cadera.', '2024-10-01 16:00:00', 5, 2),
('Consulta', 'Consulta inicial, cambio de dueño.', '2024-05-01 14:00:00', 6, 1),
('Vacunación', 'Vacuna de refuerzo.', '2024-07-15 10:00:00', 6, 3),
('Peluquería', 'Corte de pelo y uñas.', '2024-11-20 11:00:00', 6, 1),
('Vacunación', 'Vacuna inicial.', '2024-03-01 15:00:00', 7, 4),
('Consulta', 'Revisión por estornudos.', '2024-06-01 12:00:00', 7, 2),
('Consulta', 'Examen de dieta y UVB.', '2024-01-01 10:00:00', 8, 3),
('Laboratorio', 'Cultivo de heces.', '2024-07-20 13:00:00', 8, 3),
('Consulta', 'Consulta de control de peso.', '2025-02-01 11:00:00', 9, 3),
('Consulta', 'Consulta por tos crónica.', '2023-01-01 14:00:00', 10, 2),
('Laboratorio', 'Radiografía torácica.', '2023-01-15 15:00:00', 10, 2),
('Tratamiento', 'Inicio de medicación cardíaca.', '2023-02-01 16:00:00', 10, 1),
('Consulta', 'Control cardiológico.', '2024-01-01 17:00:00', 10, 2),
('Consulta', 'Dolor al caminar.', '2023-05-01 10:00:00', 11, 4),
('Cirugía', 'Cirugía de ligamento cruzado.', '2023-06-01 09:00:00', 11, 2),
('Consulta', 'Control post-operatorio (1 mes).', '2023-07-01 11:00:00', 11, 4),
('Fisioterapia', 'Sesión de hidroterapia.', '2023-09-01 14:00:00', 11, 4),
('Vacunación', 'Vacuna anual.', '2024-06-01 10:00:00', 11, 1),
('Vacunación', 'Antirrábica.', '2023-08-18 10:00:00', 12, 1),
('Consulta', 'Chequeo de rutina.', '2024-02-15 12:00:00', 12, 3),
('Desparasitación', 'Comprimido.', '2024-08-18 11:00:00', 12, 1),
('Consulta', 'Revisión por bola de pelo.', '2024-01-01 15:00:00', 13, 2),
('Vacunación', 'Primeras vacunas de cachorro.', '2024-07-01 10:00:00', 14, 1),
('Vacunación', 'Refuerzo.', '2024-08-01 10:00:00', 14, 1),
('Consulta', 'Consulta inicial exóticos.', '2023-01-01 13:00:00', 15, 3),
('Laboratorio', 'Análisis de plumas.', '2023-05-01 14:00:00', 15, 3),
('Tratamiento', 'Tratamiento respiratorio.', '2023-08-01 15:00:00', 15, 3),
('Vacunación', 'Vacuna anual.', '2024-03-22 10:00:00', 16, 4),
('Consulta', 'Revisión dental y limpieza.', '2023-10-01 10:00:00', 17, 2),
('Vacunación', 'Antirrábica.', '2024-01-01 11:00:00', 17, 1),
('Consulta', 'Control de rutina.', '2024-06-01 12:00:00', 17, 2),
('Vacunación', 'Vacuna inicial.', '2023-11-01 14:00:00', 18, 3),
('Desparasitación', 'Pipeta mensual.', '2024-01-01 15:00:00', 18, 4),
('Consulta', 'Revisión general exóticos.', '2024-04-01 16:00:00', 19, 3),
('Vacunación', 'Triple Felina.', '2024-05-01 17:00:00', 20, 1),
('Consulta', 'Revisión de pico y uñas.', '2024-07-01 10:00:00', 21, 3),
('Vacunación', 'Vacuna anual.', '2023-12-01 11:00:00', 22, 4),
('Consulta', 'Problemas digestivos.', '2024-03-01 12:00:00', 22, 2);
GO

-- ------------------------------
-- E) Turnos (60 Registros)
-- ------------------------------
INSERT INTO Turno (MascotaId, ClienteId, VeterinarioId, FechaHorario, Detalle) VALUES
(1, 1, 1, '2025-11-24 10:00:00', 'Vacuna anual'),
(4, 4, 2, '2025-11-24 11:00:00', 'Control peso y dieta'),
(8, 5, 3, '2025-11-24 12:00:00', 'Revisión caparazón'),
(10, 6, 4, '2025-11-24 15:00:00', 'Síntomas alergia'),
(11, 7, 1, '2025-11-25 10:00:00', 'Curación herida'),
(14, 8, 2, '2025-11-25 11:00:00', 'Chequeo general'),
(18, 10, 3, '2025-11-25 12:00:00', 'Control de garrapatas'),
(2, 2, 4, '2025-11-25 14:00:00', 'Vacunación y desparasitación'),
(5, 4, 1, '2025-11-26 09:00:00', 'Extracción de sangre'),
(12, 7, 2, '2025-11-26 10:00:00', 'Revisión ocular'),
(15, 8, 3, '2025-11-26 11:00:00', 'Consulta por estrés'),
(19, 10, 4, '2025-11-26 12:00:00', 'Vacuna de refuerzo'),
(3, 2, 1, '2025-11-27 10:00:00', 'Limpieza dental'),
(6, 4, 2, '2025-11-27 11:00:00', 'Control de crecimiento'),
(13, 8, 3, '2025-11-27 12:00:00', 'Chequeo dermatológico'),
(20, 10, 4, '2025-11-27 15:00:00', 'Revisión de patas'),
(9, 5, 1, '2025-11-28 09:00:00', 'Control roedor'),
(17, 10, 2, '2025-11-28 10:00:00', 'Chequeo post-operatorio'),
(22, 10, 3, '2025-11-28 11:00:00', 'Revisión alimentación'),
(7, 4, 4, '2025-11-28 12:00:00', 'Consulta general'),
(1, 1, 1, '2025-12-01 10:00:00', 'Control de rutina'),
(4, 4, 2, '2025-12-01 11:00:00', 'Vacuna anual'),
(8, 5, 3, '2025-12-01 12:00:00', 'Control de peso'),
(10, 6, 4, '2025-12-01 15:00:00', 'Revision de piel'),
(11, 7, 1, '2025-12-02 10:00:00', 'Curación'),
(14, 8, 2, '2025-12-02 11:00:00', 'Chequeo general'),
(18, 10, 3, '2025-12-02 12:00:00', 'Control de garrapatas'),
(2, 2, 4, '2025-12-02 14:00:00', 'Desparasitación'),
(5, 4, 1, '2025-12-03 09:00:00', 'Extracción de sangre'),
(12, 7, 2, '2025-12-03 10:00:00', 'Revisión ocular'),
(15, 8, 3, '2025-12-03 11:00:00', 'Consulta por estrés'),
(19, 10, 4, '2025-12-03 12:00:00', 'Vacuna de refuerzo'),
(3, 2, 1, '2025-12-04 10:00:00', 'Limpieza dental'),
(6, 4, 2, '2025-12-04 11:00:00', 'Control de crecimiento'),
(13, 8, 3, '2025-12-04 12:00:00', 'Chequeo dermatológico'),
(20, 10, 4, '2025-12-04 15:00:00', 'Revisión de patas'),
(9, 5, 1, '2025-12-05 09:00:00', 'Control roedor'),
(17, 10, 2, '2025-12-05 10:00:00', 'Chequeo post-operatorio'),
(22, 10, 3, '2025-12-05 11:00:00', 'Revisión alimentación'),
(7, 4, 4, '2025-12-05 12:00:00', 'Consulta general'),
(1, 1, 1, '2025-12-08 10:00:00', 'Control de rutina'),
(4, 4, 2, '2025-12-08 11:00:00', 'Vacuna anual'),
(8, 5, 3, '2025-12-08 12:00:00', 'Control de peso'),
(10, 6, 4, '2025-12-08 15:00:00', 'Revision de piel'),
(11, 7, 1, '2025-12-09 10:00:00', 'Curación'),
(14, 8, 2, '2025-12-09 11:00:00', 'Chequeo general'),
(18, 10, 3, '2025-12-09 12:00:00', 'Control de garrapatas'),
(2, 2, 4, '2025-12-09 14:00:00', 'Desparasitación'),
(5, 4, 1, '2025-12-10 09:00:00', 'Extracción de sangre'),
(12, 7, 2, '2025-12-10 10:00:00', 'Revisión ocular'),
(15, 8, 3, '2025-12-10 11:00:00', 'Consulta por estrés'),
(19, 10, 4, '2025-12-10 12:00:00', 'Vacuna de refuerzo'),
(3, 2, 1, '2025-12-11 10:00:00', 'Limpieza dental'),
(6, 4, 2, '2025-12-11 11:00:00', 'Control de crecimiento'),
(13, 8, 3, '2025-12-11 12:00:00', 'Chequeo dermatológico'),
(20, 10, 4, '2025-12-11 15:00:00', 'Revisión de patas'),
(9, 5, 1, '2025-12-30 09:00:00', 'Control roedor'),
(17, 10, 2, '2025-12-30 10:00:00', 'Chequeo post-operatorio'),
(22, 10, 3, '2025-12-31 11:00:00', 'Revisión alimentación'),
(7, 4, 4, '2025-12-31 12:00:00', 'Consulta general'),
(1, 1, 1, '2025-12-31 15:00:00', 'Control final de año');
GO
INSERT INTO [dbo].[Usuarios] (Email, Clave, Nombre) VALUES
('admin@admin.com', 'admin', 'admin');