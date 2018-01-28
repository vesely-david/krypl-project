import React from 'react'
import { connect } from 'react-redux'
import { IndexLink, Link } from 'react-router'
import PropTypes from 'prop-types'
import { authenticationActions } from '../../commonModules/authentication'
import { Button, Menu } from 'semantic-ui-react'
import { bindActionCreators } from 'redux'
import './PageLayout.scss'
import DuckImage from './assets/Duck.jpg'

class PageLayout extends React.Component {
  componentWillMount () {
    this.props.actions.checkTokenValidity()
  }

  render () {
    const indexLink = this.props.isAuthenticated ? (
      <Menu.Item as={IndexLink} to='/' activeClassName='page-layout__nav-item--active'>
        <img src={DuckImage} />
        Krypl project
      </Menu.Item>
    ) : (
      <Menu.Item>
        <img src={DuckImage} />
        Krypl project
      </Menu.Item>
    )
    return (
      <div>
        <Menu size='large'>
          {indexLink}
          {this.props.isAuthenticated && [
            <Menu.Item as={Link} to='real' activeClassName='page-layout__nav-item--active' name='real' />,
            <Menu.Item as={Link} to='paper' activeClassName='page-layout__nav-item--active' name='paper' />,
            <Menu.Item as={Link} to='backtest' activeClassName='page-layout__nav-item--active' name='backtest' />,
            <Menu.Menu position='right'>
              <Menu.Item>
                <Button onClick={() => this.props.actions.logout()}>Log out</Button>
              </Menu.Item>
            </Menu.Menu>
          ]}
        </Menu>
        {this.props.children}
      </div>

    )
  }
}

PageLayout.propTypes = {
  actions: PropTypes.object,
  isAuthenticated : PropTypes.bool,
  children: PropTypes.node,
}

const mapDispatchToProps = (dispatch) => {
  return {
    actions: bindActionCreators(authenticationActions, dispatch)
  }
}

const mapStateToProps = ({ isAuthenticated }) => ({
  isAuthenticated
})

export default connect(mapStateToProps, mapDispatchToProps)(PageLayout)
