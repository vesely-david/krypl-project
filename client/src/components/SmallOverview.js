import React from 'react'
import PropTypes from 'prop-types'
import { Table, Button, Icon, Segment, Dimmer, Loader } from 'semantic-ui-react'
import './styles/SmallOverview.scss'
import NewStrategyModal from './NewStrategyModal'

const StrategyList = ({
  overviewObject: { assets, overview },
  registrationPending,
  isFetching,
  forgetAllNews,
  registerStrategy,
}) => {
  const isEmpty = Object.keys(overview).length === 0
  return !isEmpty
    ? (
      <Segment>
        <Dimmer inverted active={isFetching}>
          <Loader />
        </Dimmer>
        <div className='smallOverview'>
          <div className='strategyOverviewTable'>
            <Table basic='very' verticalAlign='middle'>
              <Table.Body>
                <Table.Row>
                  <Table.Cell>Running Strategies</Table.Cell>
                  <Table.Cell>{`${overview.runningStrategiesCount}/${overview.allStrategiesCount}`}</Table.Cell>
                  <Table.Cell>
                    <NewStrategyModal
                      registrationPending={registrationPending}
                      registerStrategy={registerStrategy}
                      allAssets={assets}
                    />
                  </Table.Cell>
                </Table.Row>
                <Table.Row>
                  <Table.Cell>Opened trades</Table.Cell>
                  <Table.Cell>{`${overview.allOpenededTradesCount}/${overview.allTradesCount}`}</Table.Cell>
                  <Table.Cell />
                </Table.Row>
                <Table.Row>
                  <Table.Cell>New trades</Table.Cell>
                  <Table.Cell>{overview.allNewTradesCount}</Table.Cell>
                  <Table.Cell><Button secondary>Forget all new trades</Button></Table.Cell>
                </Table.Row>
              </Table.Body>
            </Table>
          </div>
          <div className='strategyScoreTable'>
            <Table basic='very' celled textAlign='center'>
              <Table.Header>
                <Table.Row>
                  <Table.HeaderCell />
                  <Table.HeaderCell>In game</Table.HeaderCell>
                  <Table.HeaderCell>Free</Table.HeaderCell>
                  <Table.HeaderCell>Sum</Table.HeaderCell>
                  <Table.HeaderCell>24h change</Table.HeaderCell>
                </Table.Row>
              </Table.Header>

              <Table.Body>
                <Table.Row>
                  <Table.Cell><Icon name='bitcoin' /></Table.Cell>
                  <Table.Cell>{overview.strategiesAssetBtc}</Table.Cell>
                  <Table.Cell>{overview.freeAssetBtc}</Table.Cell>
                  <Table.Cell>{overview.sumAssetBtc}</Table.Cell>
                  <Table.Cell>{overview.changeDayBtc}</Table.Cell>
                </Table.Row>
                <Table.Row>
                  <Table.Cell><Icon name='dollar' /></Table.Cell>
                  <Table.Cell>{overview.strategiesAssetUsd}</Table.Cell>
                  <Table.Cell>{overview.freeAssetUsd}</Table.Cell>
                  <Table.Cell>{overview.sumAssetUsd}</Table.Cell>
                  <Table.Cell>{overview.changeDayUsd}</Table.Cell>
                </Table.Row>
              </Table.Body>
            </Table>
          </div>
        </div>
      </Segment>
      )
    : (<div style={{ textAlign: 'center', padding: '0.5rem' }}>No info to display</div>)
}

StrategyList.propTypes = {
  overviewObject: PropTypes.object,
  forgetAllNews: PropTypes.func,
  isFetching: PropTypes.bool,
  registrationPending: PropTypes.bool,
  registerStrategy: PropTypes.func,
}

export default StrategyList
