import React, { Component } from 'react';
import AlertsContainer from './containers/AlertsContainer';
import Header from './containers/Header';
import Routes from './containers/Routes';
import Footer from './components/Footer';
import { bindActionCreators } from 'redux';
import {withRouter} from 'react-router-dom';
import { connect } from 'react-redux'
import { marketDataActions } from './actions/marketDataActions';
import 'semantic-ui-css/semantic.min.css'
import { assetActions } from './actions/assetActions';

class App extends Component {

  componentDidMount(){
    if(this.props.isAuthenticated){
      this.props.marketDataActions.getMarketData();
      this.props.assetActions.getAssets();
      this.props.marketDataActions.getCurrencyValues();
    }
  }

  render() {
    return (
      <div className="App">
        <AlertsContainer />
        <Header />
        <div>
          <Routes/>
          <Footer />
        </div>
      </div>
    );
  }
}

function mapStateToProps(state){
  return{
    isAuthenticated: state.user.isAuthenticated,
  }
}

function mapDispatchToProps(dispatch){
  return{
    marketDataActions: bindActionCreators(marketDataActions, dispatch),
    assetActions: bindActionCreators(assetActions, dispatch)
  }
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(App));
