import React from 'react'
import PropTypes from 'prop-types'
import { Button, Header, Modal, Message, Form } from 'semantic-ui-react'

class RealAssetManagerModal extends React.Component {
  constructor (props) {
    super(props)
    this.state = {
      selectedExchange: null,
      error: null,
      showMessage: false
    }
  }

  onExchangeSelect = (e, { value }) => {
    if (value === this.state.selectedExchange) return
    this.setState({
      selectedExchange:value,
    })
  }

  onMirrorClick = async (currency) => {
    const error = await this.props.mirrorExchangeAssets(this.state.selectedExchange)
    if (error !== null) this.setState({ error: error })
    else this.setState({ selectedExchange: null })
    this.setState({ showMessage: true })
  }

  render () {
    const {
      selectedExchange,
      showMessage,
      error,
    } = this.state
    const {
      assetOptions,
      mirrorExchangeAssetsFetching,
      exchangesOverviewFetching
    } = this.props
    const possibleExchanges = assetOptions ? assetOptions.map(o => ({ key: o.value, text: o.text, value: o.value })) : []
    return (
      <Modal trigger={
        <Button
          disabled={exchangesOverviewFetching || possibleExchanges.length === 0}
          color='green'
          loading={exchangesOverviewFetching}
        >
          Manage assets
        </Button>
        } closeIcon size='mini'>
        <Header icon='money' content='Asset Management' />
        <Modal.Content>
          <div className='newStrategyModalContent'>
            <Form widths='equal' loading={mirrorExchangeAssetsFetching}>
              <Form.Group>
                <Form.Select
                  fluid
                  label='Exchange'
                  placeholder='Exchange'
                  name='exchange'
                  value={selectedExchange}
                  options={possibleExchanges}
                  onChange={this.onExchangeSelect} />
                <Form.Button
                  disabled={selectedExchange == null}
                  onClick={this.onMirrorClick}
                  color='green'
                  className='modalButton'>
                  Mirror Assets
                </Form.Button>
              </Form.Group>
            </Form>
          </div>
          <Message
            success={showMessage && !error}
            error={showMessage && error}
            hidden={!showMessage}
            header={error ? `Insufficient funds for:\n ${error}` : 'Success'}
          />
        </Modal.Content>
      </Modal>
    )
  }
}

RealAssetManagerModal.propTypes = {
  assetOptions: PropTypes.array,
  mirrorExchangeAssets: PropTypes.func,
  mirrorExchangeAssetsFetching: PropTypes.bool,
  exchangesOverviewFetching: PropTypes.bool,
}

export default RealAssetManagerModal
