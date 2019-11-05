using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Diagnostics;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace MultiClassPTBR.Shared
{
    public class HardwareMonitorComponentClass : ComponentBase
    {
        [Inject]
        private IJSRuntime jSRuntime { get; set; }
                
        [Parameter]
        public double GridHeigth { get; set; }

        private const int SEC_WIDITH = 2;

        private Timer updateTimer;    
        private PointF lastPoint;

        public double CurrentWidth;
                
        private Random rand = new Random();
                
        public HardwareMonitorComponentClass()
        {
            updateTimer = new Timer(100);
            updateTimer.Elapsed += UpdateTimer_Elapsed;            
        }

        protected override void OnInitialized()
        {            
            lastPoint = new PointF(0, 0);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            jSRuntime.InvokeVoidAsync("initMonitor", false);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //TODO: get resize
            CurrentWidth = await jSRuntime.InvokeAsync<double>("GetMonitorWidth");            
            if (firstRender)
            {                
                updateTimer.Start();
            }
        }

        private async void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                await UpdateCanvas(await GetCpuUsageForProcess());
            }
            catch
            {
                updateTimer.Stop();
                updateTimer.Dispose();
            }
        }

        public async Task UpdateCanvas(double cpu)
        {            
            if (lastPoint.X >= CurrentWidth)
            {
                await jSRuntime.InvokeVoidAsync("ClearCanvas");
                lastPoint = new PointF(0, 0);
            }

            PointF newPoint = new PointF(lastPoint.X + SEC_WIDITH, (float)cpu);
            await jSRuntime.InvokeVoidAsync("PlotSinglePoint", lastPoint, newPoint);
            lastPoint = newPoint;
        }

        private async Task<double> GetCpuUsageForProcess()
        {
            var startTime = DateTime.UtcNow;
            var startCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
            await Task.Delay(100);

            var endTime = DateTime.UtcNow;
            var endCpuUsage = Process.GetCurrentProcess().TotalProcessorTime;
            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;
            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);
            return cpuUsageTotal;
        }
                
    }
}
