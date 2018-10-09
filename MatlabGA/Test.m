clear;clc; close all;

pFile = 'C:\Users\Jared\AppData\Local\TradeData\Optimization\param.txt';
rFile = 'C:\Users\Jared\AppData\Local\TradeData\Optimization\result.txt';
readyFile = 'C:\Users\Jared\AppData\Local\TradeData\Optimization\ready.txt';
delete(pFile)
delete(rFile)
delete(readyFile)

server = 'C:\dev\Trading\OptimizationServer\bin\Release\OptimizationServer.exe';
% dbFile = 'C:\Users\Jared\AppData\Local\TradeData\Stocks-10-6-18_small.db';
dbFile = 'C:\Users\Jared\AppData\Local\TradeData\Stocks-10-5-18_update.db';
cmd = [server ' ' dbFile '&'];
system(cmd);


while exist(readyFile)==0
    pause(.1);
end
delete(readyFile);

x0 = [0 0 0 0];
A = [];
b = [];
Aeq = [];
beq = [];
lb = [2 1.05 0.5 0.05];
ub = [12 1.5 0.95 0.5];

opts = optimoptions(@ga,'PlotFcn',{@gaplotbestf},...
    'PopulationSize',100);

f = @(x)RunSim(x,sim,list);
[x,fval,exitflag,output,population] = ga(@RunSim,4,A,b,Aeq,beq,lb,ub,[],opts);
