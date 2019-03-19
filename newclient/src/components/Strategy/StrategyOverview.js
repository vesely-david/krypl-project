import React from 'react'
import { Dimmer, Loader, Segment } from 'semantic-ui-react'
import { ParentSize } from '@vx/responsive';
import EvaluationGraph from '../EvaluationGraph';
import styles from '../styles/strategy.module.scss';

const StrategyOverview = ({ 
  overview = {},
  isFetching, 
  history = [],
  isHistoryFetching,
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

    </Segment>    
  )
}

export default StrategyOverview
