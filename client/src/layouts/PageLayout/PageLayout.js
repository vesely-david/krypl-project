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
  constructor (props) {
    super(props)
    this.state = {
      menuClass: null,
    }
  }
  componentWillMount () {
    this.props.actions.checkTokenValidity()
  }

  onMenuColorChange (color) {
    this.setState({
      menuClass: color
    })
  }

  render () {
    const indexLink = this.props.isAuthenticated ? (
      <Menu.Item className='mainItem' as={IndexLink} onClick={() => this.onMenuColorChange(null)} to='/' activeClassName='page-layout__nav-item--active'>
        <img src={DuckImage} />
        Krypl project
      </Menu.Item>
    ) : (
      <Menu.Item className='mainItem'>
        <img src={DuckImage} />
        Krypl project
      </Menu.Item>
    )
    return (
      <div>
        <Menu size='large' className={`mainMenu ${this.state.menuClass ? this.state.menuClass : ''}`}>
          {indexLink}
          {this.props.isAuthenticated && [
            <Menu.Item as={Link} onClick={() => this.onMenuColorChange('greenMenu')} to='real' key='realLink' activeClassName='page-layout__nav-item--active' name='real' />,
            <Menu.Item as={Link} onClick={() => this.onMenuColorChange('tealMenu')} to='paper' key='paperLink' activeClassName='page-layout__nav-item--active' name='paper' />,
            <Menu.Item as={Link} onClick={() => this.onMenuColorChange('blueMenu')} to='backtest' key='backtestLink' activeClassName='page-layout__nav-item--active' name='backtest' />,
            <Menu.Menu key='righSection' position='right'>
              <Menu.Item>
                <Button onClick={() => this.props.actions.logout()}>Log out</Button>
              </Menu.Item>
            </Menu.Menu>
          ]}
        </Menu>
        <div className='bodyElement'>
          {this.props.children}
        </div>
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
