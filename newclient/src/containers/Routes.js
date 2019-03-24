import React, { Component } from 'react';
import { connect } from 'react-redux';
import { Switch, Route, Redirect, withRouter } from 'react-router-dom';
import Home from './subpages/Home';
import Real from './subpages/Real';
import Paper from './subpages/Paper';
import Back from './subpages/Back';
import Login from './subpages/Login';
import Strategy from './subpages/Strategy';
import Account from './subpages/Account';


const PublicRoute = ({ component: Component, authenticated, ...rest }) => {
  return (
    <Route {...rest} render={
      props => !authenticated? (<Component {...props} />) : (<Redirect to={{pathname: "/"}} />)
    }/>)
};

const PrivateRoute = ({ component: Component, authenticated, ...rest }) => {
  return (
    <Route {...rest} render={
      props => authenticated? (<Component {...props} />) : (<Redirect to={{pathname: "/"}} />)
    }/>)
};

class Routes extends Component {
  render() {
    const { isAuthenticated } = this.props.user;
    return (
      <Switch>
        <Route exact path='/' component={Home}/>
        <PrivateRoute path="/real" exact authenticated={isAuthenticated} component={Real} />
        <PrivateRoute path="/papertesting" exact authenticated={isAuthenticated} component={Paper} />
        <PrivateRoute path="/backtesting" exact authenticated={isAuthenticated} component={Back}/>
        <PrivateRoute path="/:tradingMode/:strategyId" exact authenticated={isAuthenticated} component={Strategy}/>
        <PrivateRoute path="/account" exact authenticated={isAuthenticated} component={Account}/>
        {/* <PublicRoute path='/login' authenticated={isAuthenticated} component={Login} /> */}
      </Switch>
    );
  }
}

function mapStateToProps(state) {
  return { user: state.user };
}

export default withRouter(connect(mapStateToProps)(Routes));
