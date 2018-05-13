import React from 'react'
import PropTypes from 'prop-types'
import { format } from 'd3-format'
import { TimeSeries } from 'pondjs'
import _ from 'underscore'
import {
  ChartContainer,
  ChartRow,
  Charts,
  YAxis,
  LineChart,
  Legend,
  Resizable,
  styler,
} from 'react-timeseries-charts'

const style = styler([
    { key: 'btc', color: 'steelblue', width: 2 },
    { key: 'usd', color: '#F68B24', width: 2 }
])

class CrossHairs extends React.Component {
  render() {
    const { x } = this.props
    const style = { pointerEvents: 'none', stroke: '#ccc' }
    if (!_.isNull(x)) {
      return (
        <g>
          <line style={style} x1={x} y1={0} x2={x} y2={this.props.height} />
        </g>
      )
    } else {
      return <g />
    }
  }
}

const createPoints = (data) => {
  return data ? data.map(o => {
    const date = new Date(`${o.timeStamp}`)
    return [
      date.getTime(),
      o.btcValue,
      o.usdValue
    ]
  }) : []
}

class EvaluationGraph extends React.Component {
  constructor (props) {
    super(props)

    var series = new TimeSeries({
      name: 'Currency',
      columns: ['time', 'btc', 'usd'],
      points: createPoints(props.data)
    })

    this.state = {
      tracker: null,
      evaluationSeries: series,
      timerange: series.range(),
      x: null,
      y: null,
    }
  }

  handleTrackerChanged = tracker => {
    if (!tracker) {
      this.setState({ tracker, x: null })
    } else {
      this.setState({ tracker })
    }
  };

  handleTimeRangeChange = timerange => {
    this.setState({ timerange })
  }

  handleMouseMove = (x, y) => {
    this.setState({ x, y })
  };

  render () {
    const usdF = format('$,.2f')
    const btfF = format(',.8f')
    const evaluationSeries = this.state.evaluationSeries

    let usdValue, btcValue
    if (this.state.tracker) {
      const index = evaluationSeries.bisect(this.state.tracker)
      const trackerEvent = evaluationSeries.at(index)
      btcValue = `${btfF(trackerEvent.get('btc'))}`
      usdValue = `${usdF(trackerEvent.get('usd'))}`
    }
    const btcMax = evaluationSeries.max('btc')
    const usdMax = evaluationSeries.max('usd')
    const usdMin = evaluationSeries.min('usd')
    const btcMin = evaluationSeries.min('btc')

    return (
      <div>
        <div className='row'>
          <div className='col-md-12'>
            <Resizable>
              <ChartContainer
                timeRange={this.state.timerange}
                maxTime={evaluationSeries.range().end()}
                minTime={evaluationSeries.range().begin()}
                onTrackerChanged={this.handleTrackerChanged}
                onBackgroundClick={() => this.setState({ selection: null })}
                enablePanZoom
                onTimeRangeChanged={this.handleTimeRangeChange}
                onMouseMove={(x, y) => this.handleMouseMove(x, y)}
                minDuration={1000 * 60 * 60 * 24 * 30}
              >
                <ChartRow height='175'>
                  <YAxis
                    id='usdAxis'
                    min={usdMin}
                    max={usdMax}
                    width='60'
                    type='linear'
                    format='$,.2f'
                  />
                  <Charts>
                    <LineChart
                      axis='usdAxis'
                      breakLine={false}
                      series={evaluationSeries}
                      columns={['usd']}
                      style={style}
                      interpolation='curveBasis'
                      highlight={this.state.highlight}
                      onHighlightChange={highlight => this.setState({ highlight })}
                      selection={this.state.selection}
                      onSelectionChange={selection => this.setState({ selection })}
                    />
                    <LineChart
                      axis='btcAxis'
                      breakLine={false}
                      series={evaluationSeries}
                      columns={['btc']}
                      style={style}
                      interpolation='curveBasis'
                      highlight={this.state.highlight}
                      onHighlightChange={highlight => this.setState({ highlight })}
                      selection={this.state.selection}
                      onSelectionChange={selection => this.setState({ selection })}
                    />
                    {<CrossHairs x={this.state.x} />}
                  </Charts>
                  <YAxis
                    id='btcAxis'
                    min={btcMin}
                    max={btcMax}
                    width='60'
                    align='right'
                    type='linear'
                    format=',.8f'
                  />
                </ChartRow>
              </ChartContainer>
            </Resizable>
          </div>
        </div>
        <div className='row'>
          <div className='col-md-12'>
            <span>
              <Legend
                type='line'
                align='right'
                style={style}
                highlight={this.state.highlight}
                onHighlightChange={highlight => this.setState({ highlight })}
                selection={this.state.selection}
                onSelectionChange={selection => this.setState({ selection })}
                categories={[
                    { key: 'btc', label: 'Btc', value: btcValue },
                    { key: 'usd', label: 'USD', value: usdValue }
                ]}
              />
            </span>
          </div>
        </div>
      </div>
    )
  }
}

EvaluationGraph.propTypes = {
  data: PropTypes.array
}

// Export example
export default EvaluationGraph
