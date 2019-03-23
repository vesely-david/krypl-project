import React, { useState } from 'react'
import { Dimmer, Loader, Segment, Label, Button, Confirm, Icon } from 'semantic-ui-react'
import { ParentSize } from '@vx/responsive';
import EvaluationGraph from '../EvaluationGraph';
import{
  formatBtc,
  formatUsd,
} from '../../common/formaters';
import styles from '../styles/strategy.module.scss';
import moment from 'moment';
import {
  RUNNING,
} from '../../common/strategyStates';

const StrategyOverview = ({ 
  overview = {},
  isFetching, 
  history = [],
  isHistoryFetching,
  strategyAssets,
  hovered,
  stopStrategy,
}) => {
  const{
    name = '',
    id,
    tradingMode,
    strategyState,
    start,
    stop,
    openedTradesCount,
    tradesCount,
    initialValue={},
    currentValue={},
    finalValue={},
  } = overview;
  
  const duration = moment.duration((strategyState === RUNNING ? moment() : moment(stop)).diff(start));
  const [opened, setOpened] = useState(false);
  const stopStrategyConfirmation = (
    <Confirm 
      open={opened} 
      onCancel={() => setOpened(false)} 
      onConfirm={ async () => {
        const res = await stopStrategy(id, tradingMode);
        if(res) setOpened(false);
      }}
      content='Are you sure? Action cannot be taken back'
    />
  );
  return (
    <Segment>
      {stopStrategyConfirmation}
      <Dimmer active={isFetching} inverted >
        <Loader />
      </Dimmer>
      <div className={styles.strategy_heading}>
        <h2>{name}</h2>
        {strategyState === RUNNING && (
          <Button onClick={() => setOpened(true)}>Stop strategy</Button>
        )}
      </div>
      <div>{`Id: ${id}`}</div>
      <div className={styles.strategy_info_row}>
        <div>{`${strategyState === RUNNING ? 'Running': 'Runned'} for: ${duration.humanize()}`}</div>
        <div>{`Opened/All trades: ${openedTradesCount}/${tradesCount}`}</div>
      </div>
      <div className={styles.strategy_info_row}>
        <div>
          <span>Initial value</span>
          <span>
            <div>
              <Icon name='bitcoin' />
              <span>{formatBtc(initialValue.btcValue)}</span>
            </div>
            <div>
              <Icon name='dollar' />
              <span>{formatUsd(initialValue.usdValue)}</span>
            </div>            
          </span>
        </div>
        <div>
          <span>{`${strategyState === RUNNING ? 'Current': 'Final'} value`}</span>
          <span>
            <div>
              <Icon name='bitcoin' />
              <span>{formatBtc(strategyState === RUNNING ? currentValue.btcValue : finalValue.btcValue)}</span>
            </div>
            <div>
              <Icon name='dollar' />
              <span>{formatUsd(strategyState === RUNNING ? currentValue.usdValue : finalValue.usdValue)}</span>
            </div>            
          </span>
        </div>
      </div>          
      <ParentSize>
        {parent => (
          <div className={styles.evaluation_graph}>
            <Dimmer inverted active={isHistoryFetching}>
              <Loader />
            </Dimmer>
            <EvaluationGraph height={200} width={parent.width} history={history} hovered={hovered}/>
          </div>
        )}
      </ParentSize>  
      <h3>{`Strategy assets${strategyState === RUNNING ? '' : ' at the end'}`}</h3>
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
