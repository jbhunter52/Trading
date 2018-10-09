function state = plotState3(options, state, flag)
%PLOTSTATE Summary of this function goes here
%   Detailed explanation goes here

if strcmp('iter',flag)
    h = findall(gca, 'type', 'line');
    delete(h);
    plot(state.Population(:,1)',f3(state.Population(:,1)'),'o')
    hold on
    x = linspace(0,31,100);
    plot(x,f3(x),'-')
    pause(0.01);drawnow;
    cameratoolbar
end

end

