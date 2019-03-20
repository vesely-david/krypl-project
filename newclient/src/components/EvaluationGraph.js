import React from 'react';
import { Group } from '@vx/group';
import { curveBasis } from '@vx/curve';
import { LinePath } from '@vx/shape';
import { Threshold } from '@vx/threshold';
import { scaleTime, scaleLinear } from '@vx/scale';
import { AxisLeft, AxisBottom } from '@vx/axis';
import { GridRows, GridColumns } from '@vx/grid';

const date = d => new Date(d.timeStamp);
const usd = d => d.usdValue;
const btc = d => d.btcValue;

const xScaleF = data => scaleTime({
  domain: [Math.min(...data.map(date)), Math.max(...data.map(date))]
});

const yScaleF = data => scaleLinear({
  domain: [
    Math.min(...data.map(usd)),
    Math.max(...data.map(usd)),
  ],
  nice: true
});

export default function Theshold({ width, height, margin= {top: 25, left: 75, right:25, bottom:25}, history }) {
  const xMax = width - margin.left - margin.right;
  const yMax = height - margin.top - margin.bottom;

  const xScale = xScaleF(history).range([0, Math.max(0, xMax)]);
  const yScale = yScaleF(history).range([yMax, 0]);
  return (
    <div>
      <svg width={width} height={height}>
        <rect x={0} y={0} width={width} height={height} fill="#f3f3f3" rx={14} />
        <Group left={margin.left} top={margin.top}>
          <GridRows scale={yScale} width={xMax} height={yMax} stroke="#e0e0e0" />
          <GridColumns scale={xScale} width={xMax} height={yMax} stroke="#e0e0e0" />
          <line x1={xMax} x2={xMax} y1={0} y2={yMax} stroke="#e0e0e0" />
          <AxisBottom top={yMax} scale={xScale} numTicks={width > 520 ? 10 : 5} />
          <AxisLeft scale={yScale} />
          <LinePath
            data={history}
            curve={curveBasis}
            x={d => xScale(date(d))}
            y={d => yScale(usd(d))}
            stroke="#000"
            strokeWidth={1.5}
          />
          {/* <LinePath
            data={data}
            curve={curveBasis}
            x={d => xScale(date(d))}
            y={d => yScale(ny(d))}
            stroke="#000"
            strokeWidth={1.5}
          /> */}
        </Group>
      </svg>
    </div>
  );
}