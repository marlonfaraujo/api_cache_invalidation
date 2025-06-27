#!/bin/bash

echo "Running in container postgres..."

# Drop database
echo "Droping database..."
docker exec -i cacheinvalidation_database psql -U postgres -d postgres -c "drop database if exists developer;"

# Create database
echo "Creating database..."
docker exec -i cacheinvalidation_database psql -U postgres -d postgres -c "create database developer;"

# Run script.sql
echo "Running script.sql ..."
docker exec -i cacheinvalidation_database psql -U postgres -d developer < ./script.sql 

echo "Done."

#Run this script like this
#sh < ./postgres-docker.sh