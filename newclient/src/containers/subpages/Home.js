import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { assetActions } from '../../actions/assetActions';
import { userActions } from '../../actions/userActions';
import FakeAssetManagerModal from '../modals/FakeAssetManagerModal';
import RealAssetManagerModal from '../modals/RealAssetManagerModal';
import ExchangeBlock from '../../components/ExchangeBlock';
import { Segment } from 'semantic-ui-react'
import { getAssets, getGroupedAssets } from '../../selectors/assetSelectors';
import { getMarketData } from '../../selectors/marketDataSelectors';
import { getExchangesWithApiKey } from '../../selectors/userSelectors';

import styles from '../styles/subpages.module.scss';

class Home extends React.Component{

  componentDidMount(){
    if(this.props.user.isAuthenticated){
      this.props.assetActions.getAssets();
      this.props.userActions.getApiKeys()
    } 
  }

  render(){
    const{
      user: {
        isAuthenticated
      },
      groupedAssets: {
        groupedRealAssets,
        groupedPaperAssets,
      },
      assets: {
        realAssets,
        paperAssets,
      },
      assetActions: {
        updatePaperAssets,
        updateRealAssets,
      },
      marketData,
      exchanges,
    } = this.props;

    return isAuthenticated ? (
      <div className={styles.app}>
        <Segment color='green'  loading={false}>
          <div className={styles.home_segment}>
            <h3>Real Assets</h3>
            <RealAssetManagerModal
              color='green'
              assets={realAssets}
              submitAssets={updateRealAssets}
              exchanges={exchanges}
            />
          </div>
          {groupedRealAssets.length > 0 && (<div className='divider' />)}
          {groupedRealAssets.map(o => (
            <ExchangeBlock exchange={o.exchange} currencies={o.assets} key={o.exchange} />
          ))}
        </Segment>      
        <Segment color='teal' loading={false}>
          <div className={styles.home_segment}>
            <h3>Paper Assets</h3>
            <FakeAssetManagerModal
              color='teal'
              assets={paperAssets}
              submitAssets={updatePaperAssets}
              submitAssetsFetching={false}
              marketData={marketData}
            />
          </div>
          {groupedPaperAssets.length > 0 && (<div className='divider' />)}
          {groupedPaperAssets.map(o => (
            <ExchangeBlock exchange={o.exchange} currencies={o.assets} key={o.exchange} />
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
    exchanges: getExchangesWithApiKey(state),
  }
}

function mapDispatchToProps(dispatch) {
  return {
    assetActions: bindActionCreators(assetActions, dispatch),
    userActions: bindActionCreators(userActions, dispatch),
  }
}

export default connect(mapStateToProps, mapDispatchToProps)(Home);
