import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { assetActions } from '../../actions/assetActions';
import FakeAssetManagerModal from '../modals/FakeAssetManagerModal';
import ExchangeBlock from '../../components/ExchangeBlock';
import { Segment } from 'semantic-ui-react'
import { getAssets, getGroupedAssets } from '../../selectors/assetSelectors';
import { getMarketData } from '../../selectors/marketDataSelectors';

import styles from '../styles/subpages.module.scss';

class Home extends React.Component{

  componentDidMount(){
    this.props.assetActions.getAssets();
  }

  render(){
    const{
      user: {
        isAuthenticated
      },
      groupedAssets: {
        groupedPaperAssets,
      },
      assets: {
        paperAssets,
      },
      assetActions: {
        updatePaperAssets
      },
      marketData,
    } = this.props;
    return isAuthenticated ? (
      <div>
        <Segment color='teal' className='mainOverviewSegment' loading={false}>
          <div className='mainOverviewSegmanetHeading'>
            <h3>Paper Assets</h3>
            <FakeAssetManagerModal
              color='teal'
              assets={paperAssets}
              submitAssets={updatePaperAssets}
              submitAssetsFetching={false}
              marketData={marketData}
            />
          </div>
          {Object.keys(groupedPaperAssets).length > 0 && <div className='divider' />}
          {Object.keys(groupedPaperAssets).map(o => (
            <ExchangeBlock exchange={o} currencies={groupedPaperAssets[o]} key={o.id} />
          ))}
        </Segment>
      </div>
    ) : null
  }
}

function mapStateToProps(state) {
  return{
    user: state.user,
    assets: getAssets(state),
    groupedAssets: getGroupedAssets(state),
    marketData: getMarketData(state),
  }
}

function mapDispatchToProps(dispatch) {
  return {
    assetActions: bindActionCreators(assetActions, dispatch)
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(Home);
