#!/bin/bash
wait_time=20s
password=MssqlPass123!

echo creating resources in $wait_time
sleep $wait_time
echo starting...

echo 'creating Names DB'
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $password -i ./init.sql

for entry in "table/*.sql"
do
  echo executing $entry
  /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $password -i $entry
done

for entry in "data/*.sql"
do
  echo executing $entry
  /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $password -i $entry
done

