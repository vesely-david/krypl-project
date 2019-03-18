import React, { Component } from 'react';
import { connect } from 'react-redux';
import { Switch, Route, Redirect, withRouter } from 'react-router-dom';
import Home from './subpages/Home';
import Real from './subpages/Real';
import Paper from './subpages/Paper';
import Back from './subpages/Back';
import Login from './subpages/Login';
import Strategy from './subpages/Strategy';


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
        <PrivateRoute path="/real" authenticated={isAuthenticated} component={Real} />
        <PrivateRoute path="/papertesting" authenticated={isAuthenticated} component={Paper} />
        <PrivateRoute path="/backtesting" authenticated={isAuthenticated} component={Back} />
        <PrivateRoute path="/strategy/:strategyId" authenticated={isAuthenticated} component={Strategy} />
        <PublicRoute path='/login'  authenticated={isAuthenticated} component={Login} />
      </Switch>
    );
  }
}

function mapStateToProps(state) {
  return { user: state.user };
}

export default withRouter(connect(mapStateToProps)(Routes));
