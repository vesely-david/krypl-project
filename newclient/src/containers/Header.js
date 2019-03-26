import React from 'react';
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { Button, Menu } from 'semantic-ui-react'
import { NavLink, withRouter } from 'react-router-dom'
import {userActions} from '../actions/userActions';
import LoginModal from './modals/LoginModal';
import RegisterModal from './modals/RegisterModal';

import DuckIcon from '../content/images/duckling.svg'
import styles from './styles/header.module.scss';

class Header extends React.Component{

  render(){
    const indexLink = this.props.user.isAuthenticated ? (
      <Menu.Item className={styles.mainItem} as={NavLink} to='/' activeClassName={styles.active_navlink}>
        <img alt="" src={DuckIcon} style={{height: '46px', width: '46px'}}/>
        K____ p__
      </Menu.Item>
    ) : (
      <Menu.Item className={styles.mainItem}>
        <img alt="" src={DuckIcon} style={{height: '46px', width: '46px'}}/>
        K____ p__
      </Menu.Item>
    )

    return (
      <Menu size='large' className={styles.header}>
        {indexLink}
        {this.props.user.isAuthenticated ? 
          (
            <React.Fragment>
              <Menu.Item as={NavLink} to='/real' key='realLink' activeClassName={styles.active_navlink} name='real'/>
              <Menu.Item as={NavLink} to='/papertesting' key='paperLink' activeClassName={styles.active_navlink} name='paper'/>
              {/* <Menu.Item as={NavLink} to='/backtesting' key='backtestLink' activeClassName={styles.active_navlink} name='backtest'/> */}
              <Menu.Menu key='righSection' position='right'>
                <Menu.Item as={NavLink} to='/account' key='account' activeClassName={styles.active_navlink} name='account'/>           
                <Menu.Item>
                  <Button onClick={() => this.props.userActions.logout()}>Log out</Button>
                </Menu.Item>
              </Menu.Menu>
            </React.Fragment>
          ) : (
            <React.Fragment>
              <Menu.Menu key='righSection' position='right'>
                <Menu.Item>
                  <LoginModal loginAction={this.props.userActions.login}/>
                </Menu.Item>
                <Menu.Item>
                  <RegisterModal registerAction={this.props.userActions.register}/>
                </Menu.Item>                
              </Menu.Menu>
            </React.Fragment>
          )
        }
      </Menu>
    )
  }
}


function mapStateToProps(state) {
  return{
    user: state.user,
  }
}

function mapDispatchToProps(dispatch) {
  return {
    userActions: bindActionCreators(userActions, dispatch)
  }
}

export default withRouter(connect(mapStateToProps, mapDispatchToProps)(Header))