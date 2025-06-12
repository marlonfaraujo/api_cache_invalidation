drop database if exists developer;

create database developer;

create table products (
	id uuid not null,
	name text not null,
	description text not null,
	status text not null,
	price numeric not null,
	created_at timestamptz not null,
	updated_at timestamptz,
	deleted_at timestamptz,
	primary key (id)
);

/*

INSERT INTO products (id, name, description, status, price, created_at, updated_at, deleted_at) VALUES
('550e8400-e29b-41d4-a716-446655440000', 'Notebook Pro X', 'Notebook com 16GB RAM, SSD 512GB, i7', 'active', 5899.90, now(), now(), null),
('660e8400-e29b-41d4-a716-446655440001', 'Smartphone Z5', 'Smartphone com câmera tripla e 5G', 'active', 2999.00, now(), now(), null),
('770e8400-e29b-41d4-a716-446655440002', 'Monitor UltraWide', 'Monitor 34" UltraWide para produtividade', 'inactive', 1999.50, now(), now(), null),
('880e8400-e29b-41d4-a716-446655440003', 'Teclado Mecânico RGB', 'Teclado com switches red, iluminação RGB', 'active', 399.90, now(), now(), null),
('990e8400-e29b-41d4-a716-446655440004', 'Cadeira Ergonômica Pro', 'Cadeira com suporte lombar e ajustes 3D', 'deleted', 1299.00, now(), now(), now());


*/