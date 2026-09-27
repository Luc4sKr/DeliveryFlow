package ports

import "context"

type EventPublisher interface {
	Publish(context.Context, []byte) error
}
