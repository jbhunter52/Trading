function [ value ] = RunSim(x)
%TESTF Summary of this function goes here
%   Detailed explanation goes here

pFile = 'C:\Users\Jared\AppData\Local\TradeData\Optimization\param.txt';
rFile = 'C:\Users\Jared\AppData\Local\TradeData\Optimization\result.txt';

fileID = fopen(pFile,'w');
fprintf(fileID,'%f,%f,%f,%f',x);
fclose(fileID);

while exist(rFile)==0
    pause(.1);
end

fileID = fopen(rFile,'r');
value = fscanf(fileID,'%f');
fclose(fileID);
delete(rFile);

value = -value;

end

