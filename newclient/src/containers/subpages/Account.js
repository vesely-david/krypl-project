import React from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import { userActions } from '../../actions/userActions'
import { Segment, Button } from 'semantic-ui-react';
import ApiKey from '../../components/Account/ApiKey';
import { getExchangesWithApiKey } from '../../selectors/userSelectors';
import styles from '../styles/subpages.module.scss';


class AccountContainer extends React.Component {
  componentDidMount () {
    this.props.userActions.getApiKeys()
  }

  render () {
    const {
      userActions,
      exchanges,
      isAccountInfoFetching,
      isApiKeyFetching,
    } = this.props;

    return (
      <div className={styles.app}>
        <Segment loading={isAccountInfoFetching}>
          <div className={styles.heading_with_button}>
            <h2>Account settings</h2>
            <Button primary>Edit</Button>
          </div>
        </Segment>
        <Segment loading={isApiKeyFetching}>
          <div className={styles.heading_with_button}>
            <h2>Stored API keys - READ ONLY ONES!!!</h2>
          </div>
          {exchanges.map(o => (
            <ApiKey 
              key={o.exchangeId} 
              {...o}
              onDelete={userActions.deleteApiKey}
              onEdit={userActions.editApiKey}
            />
          ))}
        </Segment>
      </div>
    )
  }
}

const mapDispatchToProps = (dispatch) => {
  return {
    userActions: bindActionCreators(userActions, dispatch),
  }
}

const mapStateToProps = (state) => ({
  exchanges: getExchangesWithApiKey(state),
  isAccountInfoFetching: state.user.isFetching,
  apiKeys: state.user.apiKeys,
  isApiKeyFetching: state.user.isApiKeyFetching,
})

export default connect(mapStateToProps, mapDispatchToProps)(AccountContainer)
