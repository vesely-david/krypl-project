import React from 'react'
import PropTypes from 'prop-types'
import { Button, Header, Modal, Message, Form } from 'semantic-ui-react'

class AssetManagerModal extends React.Component {
  constructor (props) {
    super(props)
    this.state = {
      selectedExchange: null,
    }
  }

  onExchangeSelect = (e, { value }) => {
    if (value === this.state.selectedExchange) return
    this.setState({
      selectedExchange:value,
    })
  }

  onMirrorClick = (currency) => {
    this.setState({ assetOptions: this.state.assetOptions.map(
      o => o.value === currency ? { ...o, available: true, taken: 0 } : o) })
  }

  render () {
    const {
      selectedExchange,
    } = this.state
    const {
      assetOptions,
      assetOptionsFetching,
      assetUpdateFetching
    } = this.props
    const possibleExchanges = assetOptions.map(o => ({ key: o.value, text: o.text, value: o.value }))
    return (
      <Modal trigger={<Button primary>Manage</Button>} closeIcon>
        <Header icon='money' content='Asset Manager' />
        <Modal.Content>
          <div className='newStrategyModalContent'>
            <Form loading={assetOptionsFetching || assetUpdateFetching}>
              <Form.Group>
                <Form.Select
                  fluid
                  width={5}
                  label='Exchange'
                  placeholder='Exchange'
                  name='exchange'
                  value={selectedExchange}
                  options={possibleExchanges}
                  onChange={this.onExchangeSelect} />
                <Form.Button
                  disabled={selectedExchange == null}
                  onClick={this.onAddAsset}
                  color='green'
                  className='modalButton'>
                  Mirror Assets
                </Form.Button>
              </Form.Group>
            </Form>
          </div>
          <Message
            success={returnedId && returnedId > 0}
            error={returnedId && returnedId <= 0}
            hidden={returnedId === null}
            header={returnedId && returnedId > 0 ? `Strategy Registered. Id: ${returnedId}` : 'Error getting id'}
          />
        </Modal.Content>
      </Modal>
    )
  }
}

AssetManagerModal.propTypes = {
  assetOptions: PropTypes.array,
  assetOptionsFetching: PropTypes.bool,
  assetUpdateFetching: PropTypes.bool
}

export default AssetManagerModal
