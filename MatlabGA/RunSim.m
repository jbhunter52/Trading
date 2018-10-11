function [ value ] = RunSim(x)
%TESTF Summary of this function goes here
%   Detailed explanation goes here

pFile = 'C:\Users\Jared\AppData\Local\TradeData\Optimization\param.txt';
rFile = 'C:\Users\Jared\AppData\Local\TradeData\Optimization\result.txt';


minTotalRank = x(1);
minSingleRank = (x(1)/3) * x(2);
stopGain = x(3);
stopLoss = x(4);
minGrowth = x(5);

fileID = fopen(pFile,'w');
fprintf(fileID,'%f,%f,%f,%f,%f',minTotalRank, minSingleRank, stopGain, stopLoss, minGrowth);
fclose(fileID);

while exist(rFile)==0
    pause(.1);
end

fileID = fopen(rFile,'r');
value = fscanf(fileID,'%f');
fclose(fileID);
delete(rFile);

%Write log
logFile = 'C:\Users\Jared\AppData\Local\TradeData\Optimization\log.txt';
fileID = fopen(logFile,'a+');
fprintf(fileID,'%f,%f,%f,%f,%f,%f',minTotalRank, minSingleRank, stopGain, stopLoss, minGrowth, value);
fclose(fileID);

value = -value;

end

