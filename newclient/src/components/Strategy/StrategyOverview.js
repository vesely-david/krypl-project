import React from 'react'
import { Dimmer, Loader, Segment, Label } from 'semantic-ui-react'
import { ParentSize } from '@vx/responsive';
import EvaluationGraph from '../EvaluationGraph';
import styles from '../styles/strategy.module.scss';

const StrategyOverview = ({ 
  overview = {},
  isFetching, 
  history = [],
  isHistoryFetching,
  strategyAssets,
}) => {
  const{
    name = '',
  } = overview;
  return (
    <Segment>
      <Dimmer active={isFetching} inverted >
        <Loader />
      </Dimmer>
      <h2>{name}</h2>
      <ParentSize>
        {parent => (
          <div className={styles.evaluation_graph}>
            <Dimmer inverted active={isHistoryFetching}>
              <Loader />
            </Dimmer>
            <EvaluationGraph height={200} width={parent.width} history={history}/>
          </div>
        )}
      </ParentSize>
      <h3>Strategy assets</h3>
      <div>
        {strategyAssets.map(curr => (
          <Label className='currencyLabel' key={curr.id}>
            {`${curr.currency} ${curr.amount}`}
          </Label>
        ))}
      </div>
    </Segment>    
  )
}

export default StrategyOverview
